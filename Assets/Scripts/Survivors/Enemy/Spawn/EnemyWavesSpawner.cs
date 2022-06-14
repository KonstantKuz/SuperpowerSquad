using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Spawn
{
    public class EnemyWavesSpawner : MonoBehaviour
    {
        private const string ENEMY_LAYER_NAME = "Enemy";
        private static int ENEMY_LAYER;
        
        [Range(0f,1f)]
        [SerializeField] private float _moveDirectionDrivenPlaceChance = 0.3f;
        [SerializeField] private int _angleAttemptCount = 3;
        [SerializeField] private int _rangeAttemptCount = 3;
        [SerializeField] private float _minOutOfViewOffset = 2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        private ISpawnPlaceProvider _randomDrivenProvider;
        private ISpawnPlaceProvider _moveDirectionDrivenProvider;
        private SpawnerDebugger _spawnerDebugger;
        
        [Inject] private World _world;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        private SpawnerDebugger Debugger => _spawnerDebugger ??= gameObject.AddComponent<SpawnerDebugger>();
        
        private void Awake()
        {
            ENEMY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public void StartSpawn(EnemyWavesConfig enemyWavesConfig)
        {
            Stop();
            InitPlaceProviders();
            var orderedConfigs = enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime);
            _waves = new List<EnemyWaveConfig>(orderedConfigs);
            _spawnCoroutine = StartCoroutine(SpawnWaves());
        }

        private void InitPlaceProviders()
        {
            _randomDrivenProvider = new RandomDrivenPlaceProvider(this, _world);
            _moveDirectionDrivenProvider = new MoveDirectionDrivenPlaceProvider(this, _world.Squad);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
        }
        private IEnumerator SpawnWaves()
        {
            var currentTime = 0;
            foreach (var wave in _waves)
            {
                yield return new WaitForSeconds(wave.SpawnTime - currentTime);
                currentTime = wave.SpawnTime; 
                SpawnNextWave(wave);
            } 
            Stop();
        }
        
        private void SpawnNextWave(EnemyWaveConfig wave)
        {
            var place = GetPlaceForWave(wave);
            SpawnWave(wave, place);
        }

        public void SpawnWave(EnemyWaveConfig wave, SpawnPlace spawnPlace)
        {
            if (!spawnPlace.IsValid)
            {
                Debug.LogWarning("Invalid spawn place provided. Spawn wave has been canceled.");
                return;
            }
            
            for (int i = 0; i < wave.Count; i++)
            {
                SpawnEnemy(spawnPlace.Position, wave);
            }
        }

        public SpawnPlace GetPlaceForWave(EnemyWaveConfig waveConfig)
        {
            var placeProvider = Random.value < _moveDirectionDrivenPlaceChance ?
                _moveDirectionDrivenProvider : _randomDrivenProvider;
            
            return FindEmptyPlace(placeProvider, waveConfig);
        }

        private SpawnPlace FindEmptyPlace(ISpawnPlaceProvider placeProvider, EnemyWaveConfig waveConfig)
        {
            for (int rangeTry = 1; rangeTry <= _rangeAttemptCount; rangeTry++)
            {
                for (int angleTry = 0; angleTry < _angleAttemptCount; angleTry++)
                {
                    var spawnPlace = placeProvider.GetSpawnPlace(waveConfig, rangeTry);
                    if (spawnPlace.IsValid)
                    {
                        return spawnPlace;
                    }
                }
            }

            return SpawnPlace.INVALID;
        }

        public float GetOutOfViewOffset(EnemyWaveConfig waveConfig, int rangeTry)
        {
            return _minOutOfViewOffset + rangeTry * GetWaveRadius(waveConfig);
        }

        private float GetWaveRadius(EnemyWaveConfig waveConfig)
        {
            var enemyConfig = _enemyUnitConfigs.Get(waveConfig.EnemyId);
            return Mathf.Sqrt(waveConfig.Count) * enemyConfig.GetScaleForLevel(waveConfig.EnemyLevel);
        }

        public bool IsPlaceBusy(Vector3 place, EnemyWaveConfig waveConfig)
        {
            var isBusy = Physics.CheckSphere(place, GetWaveRadius(waveConfig), 1 << ENEMY_LAYER);
            Debugger.Debug(place, GetWaveRadius(waveConfig), isBusy);
            return isBusy;
        }

        private void SpawnEnemy(Vector3 place, EnemyWaveConfig wave)
        {
            var enemy = _unitFactory.CreateEnemy(wave.EnemyId, wave.EnemyLevel);
            var enemyAi = enemy.GetComponent<EnemyAi>();
            enemyAi.NavMeshAgent.Warp(place);
        }

        private void Stop()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
        private void OnDestroy()
        {
            _messenger.Unsubscribe<SessionEndMessage>(OnSessionFinished);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
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
        public static readonly Vector3 INVALID_SPAWN_PLACE = Vector3.one * int.MaxValue;

        private const string ENEMY_LAYER_NAME = "Enemy";
        private static int ENEMY_LAYER;
        
        [Range(0f,1f)]
        [SerializeField] private float _moveDirectionDrivenPlaceChance = 0.3f;
        [SerializeField] private int _findPlaceAttemptCount = 3;
        [SerializeField] private int _maxOutOfViewMultiplier = 3;
        [SerializeField] private float _minOutOfViewOffset = 2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        private ISpawnPlaceProvider _randomDrivenPlaceProvider;
        private ISpawnPlaceProvider _moveDirectionDrivenPlaceProvider;
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
            _randomDrivenPlaceProvider = new RandomDrivenPlaceProvider(this, _world);
            _moveDirectionDrivenPlaceProvider = new MoveDirectionDrivenPlaceProvider(this, _world.Squad);
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

        public void SpawnWave(EnemyWaveConfig wave, Vector3 place)
        {
            if (place == INVALID_SPAWN_PLACE)
            {
                Debug.LogWarning("Invalid spawn place provided. Spawn wave has been canceled.");
                return;
            }
            
            for (int i = 0; i < wave.Count; i++)
            {
                SpawnEnemy(place, wave);
            }
        }

        public Vector3 GetPlaceForWave(EnemyWaveConfig waveConfig)
        {
            var placeProvider = Random.value < _moveDirectionDrivenPlaceChance ?
                _moveDirectionDrivenPlaceProvider : _randomDrivenPlaceProvider;
            
            return FindEmptyPlace(placeProvider, waveConfig);
        }

        private Vector3 FindEmptyPlace(ISpawnPlaceProvider placeProvider, EnemyWaveConfig waveConfig)
        {
            for (int outOfViewMultiplier = 1; outOfViewMultiplier <= _maxOutOfViewMultiplier; outOfViewMultiplier++)
            {
                for (int attemptCount = 1; attemptCount < _findPlaceAttemptCount; attemptCount++)
                {
                    var spawnPlace = placeProvider.GetSpawnPlace(waveConfig, outOfViewMultiplier);
                    if (!IsPlaceBusy(spawnPlace, waveConfig))
                    {
                        return spawnPlace;
                    }
                }
            }

            return INVALID_SPAWN_PLACE;
        }

        public float GetOutOfViewOffset(EnemyWaveConfig waveConfig, int outOfViewMultiplier)
        {
            return outOfViewMultiplier * (_minOutOfViewOffset + GetWaveRadius(waveConfig));
        }

        private float GetWaveRadius(EnemyWaveConfig waveConfig)
        {
            var enemyConfig = _enemyUnitConfigs.Get(waveConfig.EnemyId);
            return Mathf.Sqrt(waveConfig.Count) * enemyConfig.GetScaleForLevel(waveConfig.EnemyLevel);
        }

        private bool IsPlaceBusy(Vector3 place, EnemyWaveConfig waveConfig)
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

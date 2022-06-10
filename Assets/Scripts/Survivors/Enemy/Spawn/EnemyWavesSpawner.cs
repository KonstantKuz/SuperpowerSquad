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
        public const int INITIAL_SPAWN_OFFSET_MULTIPLIER = 1;
        public static readonly Vector3 INVALID_SPAWN_PLACE = Vector3.one * 12345;

        private static int ENEMY_LAYER;

        [Range(0f,1f)]
        [SerializeField] private float _destinationDrivenPlaceChance = 0.3f;
        [SerializeField] private int _findPlaceAttemptCount = 3;
        [SerializeField] private float _minOutOfViewOffset = 2f;
        [SerializeField] private float _outOfViewOffsetMultiplier = 0.2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        private IEnemySpawnPlaceProvider _randomDrivenPlaceProvider;
        private IEnemySpawnPlaceProvider _destinationDrivenPlaceProvider;
        
        [Inject] private World _world;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;

        private void Awake()
        {
            ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
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
            _destinationDrivenPlaceProvider = new DestinationDrivenPlaceProvider(this, _world.Squad);
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
                Debug.Log("Empty place was not found. Spawn wave has been canceled.");
                return;
            }
            
            for (int i = 0; i < wave.Count; i++)
            {
                SpawnEnemy(place, wave);
            }
        }

        public Vector3 GetPlaceForWave(EnemyWaveConfig waveConfig)
        {
            var placeProvider = Random.value < _destinationDrivenPlaceChance ?
                _destinationDrivenPlaceProvider : _randomDrivenPlaceProvider;
            
            return GetEmptySpawnPlaceRecursive(placeProvider,waveConfig, 1, INITIAL_SPAWN_OFFSET_MULTIPLIER);
        }
        
        private Vector3 GetEmptySpawnPlaceRecursive(IEnemySpawnPlaceProvider placeProvider, EnemyWaveConfig waveConfig, int attemptCount, int spawnOffsetMultiplier)
        {
            var spawnPlace = placeProvider.GetSpawnPlace(waveConfig, spawnOffsetMultiplier);
            if (!IsPlaceBusy(spawnPlace, waveConfig))
            {
                return spawnPlace;
            }
            
            attemptCount++;
            if (attemptCount > _findPlaceAttemptCount)
            {
                attemptCount = 1;
                spawnOffsetMultiplier++;
                if (spawnOffsetMultiplier > _findPlaceAttemptCount)
                {
                    return INVALID_SPAWN_PLACE;
                }
            }

            return GetEmptySpawnPlaceRecursive(placeProvider, waveConfig, attemptCount, spawnOffsetMultiplier);
        }

        public float GetSpawnOffset(EnemyWaveConfig waveConfig)
        {
            return _minOutOfViewOffset + GetWaveRadius(waveConfig) * _outOfViewOffsetMultiplier;
        }

        private float GetWaveRadius(EnemyWaveConfig waveConfig)
        {
            var enemyConfig = _enemyUnitConfigs.Get(waveConfig.EnemyId);
            return Mathf.Sqrt(waveConfig.Count) * enemyConfig.GetScaleForLevel(waveConfig.EnemyLevel);
        }

        private bool IsPlaceBusy(Vector3 place, EnemyWaveConfig waveConfig)
        {
            var waveRadius = GetWaveRadius(waveConfig);
            var status = Physics.CheckSphere(place, waveRadius, 1 << ENEMY_LAYER);
            _spheres[place] = waveRadius;
            _lifetimes[place] = 2;
            _statuses[place] = status;
            return status;
        }

        private Dictionary<Vector3, float> _spheres = new Dictionary<Vector3, float>();
        private Dictionary<Vector3, float> _lifetimes = new Dictionary<Vector3, float>();
        private Dictionary<Vector3, bool> _statuses = new Dictionary<Vector3, bool>();
        private void OnDrawGizmos()
        {
            foreach (var sphere in _spheres)
            {
                _lifetimes[sphere.Key] -= Time.deltaTime;
                if (_lifetimes[sphere.Key] <= 0)
                {
                    continue;
                }
                var color = _statuses[sphere.Key] ? Color.red : Color.green;
                color.a = 0.3f;
                Gizmos.color = color;
                Gizmos.DrawSphere(sphere.Key, sphere.Value);
            }
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

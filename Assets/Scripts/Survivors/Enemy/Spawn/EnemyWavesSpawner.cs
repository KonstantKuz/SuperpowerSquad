using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Extension;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn
{
    public class EnemyWavesSpawner : MonoBehaviour
    {
        private const string ENEMY_LAYER_NAME = "Enemy";
        private static int ENEMY_LAYER;
        private static readonly Vector3 INVALID_SPAWN_PLACE = Vector3.one * int.MaxValue;
        
        [SerializeField] private int _findPlaceAttemptCount = 3;
        [SerializeField] private int _maxOutOfViewMultiplier = 3;
        [SerializeField] private float _minOutOfViewOffset = 2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        private SpawnerDebugger _spawnerDebugger;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;
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
            var orderedConfigs = enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime);
            _waves = new List<EnemyWaveConfig>(orderedConfigs);
            _spawnCoroutine = StartCoroutine(SpawnWaves());
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

        public Vector3 GetPlaceForWave(EnemyWaveConfig wave)
        {
            var enemyConfig = _enemyUnitConfigs.Get(wave.EnemyId);
            var waveRadius = Mathf.Sqrt(wave.Count) * enemyConfig.GetScaleForLevel(wave.EnemyLevel);
            var outOfViewOffset = _minOutOfViewOffset + waveRadius;
            
            return FindRandomEmptyPlace(outOfViewOffset, waveRadius);
        }

        private Vector3 FindRandomEmptyPlace(float outOfViewOffset, float waveRadius)
        {
            for (int outOfViewMultiplier = 1; outOfViewMultiplier <= _maxOutOfViewMultiplier; outOfViewMultiplier++)
            {
                for (int attemptCount = 1; attemptCount < _findPlaceAttemptCount; attemptCount++)
                {
                    outOfViewOffset *= outOfViewMultiplier;
                    var spawnSide = EnumExt.GetRandom<SpawnSide>();
                    var spawnPlace = GetRandomSpawnPlace(spawnSide, outOfViewOffset);
                    if (!IsPlaceBusy(spawnPlace, waveRadius))
                    {
                        return spawnPlace;
                    }
                }
            }

            return INVALID_SPAWN_PLACE;
        }

        private Vector3 GetRandomSpawnPlace(SpawnSide spawnSide, float outOfViewOffset)
        {
            var camera = UnityEngine.Camera.main.transform;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.forward, _world.Ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.right, _world.Ground.up).normalized;

            var randomPlace = GetRandomPlaceOnGround(spawnSide);
            randomPlace += spawnSide switch
            {
                SpawnSide.Top => directionToTopSide * outOfViewOffset,
                SpawnSide.Bottom => -directionToTopSide * outOfViewOffset,
                SpawnSide.Right => directionToRightSide * outOfViewOffset,
                SpawnSide.Left => -directionToRightSide * outOfViewOffset,
                _ => Vector3.zero
            };
            return randomPlace;
        }

        private Vector3 GetRandomPlaceOnGround(SpawnSide spawnSide)
        {
            var camera = UnityEngine.Camera.main;
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            var place = _world.GetGroundIntersection(pointRay);
            return place;
        }

        private Vector2 GetRandomPointOnViewportEdge(SpawnSide spawnSide)
        {
            switch (spawnSide)
            {
                case SpawnSide.Top:
                    return new Vector2(Random.Range(0f, 1f), 1f);
                case SpawnSide.Bottom:
                    return new Vector2(Random.Range(0f, 1f), 0f);
                case SpawnSide.Right:
                    return new Vector2(1f, Random.Range(0f, 1f));
                case SpawnSide.Left:
                    return new Vector2(0f, Random.Range(0f, 1f));
                default:
                    throw new ArgumentException("Unexpected spawn side");
            }
        }

        private bool IsPlaceBusy(Vector3 place, float waveRadius)
        {
            var isBusy = Physics.CheckSphere(place, waveRadius, 1 << ENEMY_LAYER);
            Debugger.Debug(place, waveRadius, isBusy);
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
        private enum SpawnSide
        {
            Top,
            Bottom,
            Right,
            Left,
        }
    }
}

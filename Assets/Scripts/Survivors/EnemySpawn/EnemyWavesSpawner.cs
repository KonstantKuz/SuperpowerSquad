using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Extension;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.EnemySpawn
{
    public class EnemyWavesSpawner : MonoBehaviour, IWorldScope
    {
        private static int ENEMY_LAYER;
        private static readonly Vector3 INVALID_SPAWN_PLACE = Vector3.one * 12345;
        
        [SerializeField] private int _maxFindPlaceAttemptCount = 5;
        [SerializeField] private float _minOutOfViewOffset = 2f;
        [SerializeField] private float _outOfViewOffsetMultiplier = 0.2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;

        private void Awake()
        {
            ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        }

        public void OnWorldSetup()
        {
            
        }
        public void StartSpawn(EnemyWavesConfig enemyWavesConfig)
        {
            Dispose();
            var orderedConfigs = enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime);
            _waves = new List<EnemyWaveConfig>(orderedConfigs);
            _spawnCoroutine = StartCoroutine(SpawnWaves());
        }
        public void OnWorldCleanUp()
        {
            Dispose();
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
            Dispose();
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
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            var spawnOffset = _minOutOfViewOffset + waveRadius * _outOfViewOffsetMultiplier;

            var spawnPlace = GetSpawnPlace(spawnSide, spawnOffset);

            var attemptCount = 1;
            var spawnOffsetMultiplier = 1;
            while (IsPlaceBusy(spawnPlace, waveRadius) && spawnOffsetMultiplier <= _maxFindPlaceAttemptCount)
            {
                if (attemptCount > _maxFindPlaceAttemptCount)
                {
                    attemptCount = 1;
                    spawnOffsetMultiplier++;
                }
                
                attemptCount++;
                spawnOffset *= spawnOffsetMultiplier;
                spawnSide = EnumExt.GetRandom<SpawnSide>();
                spawnPlace = GetSpawnPlace(spawnSide, spawnOffset);
            }

            return IsPlaceBusy(spawnPlace, waveRadius) ? INVALID_SPAWN_PLACE : spawnPlace;
        }

        private Vector3 GetSpawnPlace(SpawnSide spawnSide, float spawnOffset)
        {
            var camera = UnityEngine.Camera.main.transform;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.forward, _world.Ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.right, _world.Ground.up).normalized;

            var randomPlace = GetRandomPlaceOnGround(spawnSide);
            randomPlace += spawnSide switch
            {
                SpawnSide.Top => directionToTopSide * spawnOffset,
                SpawnSide.Bottom => -directionToTopSide * spawnOffset,
                SpawnSide.Right => directionToRightSide * spawnOffset,
                SpawnSide.Left => -directionToRightSide * spawnOffset,
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

        private void Dispose()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
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

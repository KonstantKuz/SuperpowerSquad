using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session;
using Survivors.Squad;
using Survivors.Units.Enemy;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.EnemySpawn
{
    public class EnemyWavesSpawner : MonoBehaviour, IWorldScope
    {
        [SerializeField] private float _minOutOfViewOffset = 2f;
        [SerializeField] private float _outOfViewOffsetMultiplier = 0.2f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;

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
                SpawnNextWave(wave.Count, 1);
            } 
            Dispose();
        }
        
        public void SpawnNextWave(int spawnCount, int level)
        {
            var place = GetRandomPlaceForWave(spawnCount * _outOfViewOffsetMultiplier);
            for (int i = 0; i < spawnCount; i++) 
            {
                SpawnEnemy(place, level);
            }
        }
        
        private Vector3 GetRandomPlaceForWave(float waveRadius)
        {
            var camera = UnityEngine.Camera.main;
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            var place = _world.GetGroundIntersection(pointRay);
            return GetSpawnPlaceWithOffset(place, spawnSide, waveRadius);
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

        private Vector3 GetSpawnPlaceWithOffset(Vector3 place, SpawnSide spawnSide, float waveRadius)
        {
            var camera = UnityEngine.Camera.main;
            var spawnOffset = _minOutOfViewOffset + waveRadius;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.transform.forward, _world.Ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.transform.right, _world.Ground.up).normalized;

            place += spawnSide switch
            {
                SpawnSide.Top => directionToTopSide * spawnOffset,
                SpawnSide.Bottom => -directionToTopSide * spawnOffset,
                SpawnSide.Right => directionToRightSide * spawnOffset,
                SpawnSide.Left => -directionToRightSide * spawnOffset,
                _ => Vector3.zero
            };

            return place;
        }

        private void SpawnEnemy(Vector3 place, int level)
        {
            var enemy = _unitFactory.CreateEnemy(level);
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

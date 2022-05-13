using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using LegionMaster.Extension;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.EnemySpawn
{
    public class EnemyWavesSpawner : MonoBehaviour
    {
        [SerializeField] private float _minOutOfViewOffset = 1f;
        [SerializeField] private float _outOfViewOffsetMultiplier = 0.3f;

        private List<EnemyWaveConfig> _waves;
        private Coroutine _spawnCoroutine;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;

        public void StartSpawn(EnemyWavesConfig enemyWavesConfig)
        {
            if (_spawnCoroutine != null)
            {
                Dispose();
            }

            var orderedConfigs = enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime);
            _waves = new List<EnemyWaveConfig>(orderedConfigs);
            _spawnCoroutine = StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            var currentTime = 0;
            foreach (var wave in _waves)
            {
                yield return new WaitForSeconds(wave.SpawnTime - currentTime);
                currentTime = wave.SpawnTime; 
                SpawnNextWave(wave.Count);
            } 
            Dispose();
        }
        
        private void SpawnNextWave(int spawnCount)
        {
            var place = GetRandomPlaceForWave(spawnCount * _outOfViewOffsetMultiplier);
            for (int i = 0; i < spawnCount; i++) 
            {
                SpawnEnemy(place);
            }
        }
        
        private Vector3 GetRandomPlaceForWave(float waveRadius)
        {
            var camera = UnityEngine.Camera.main;
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            var plane = new Plane(_world.Player.up, _world.Player.position);
            plane.Raycast(pointRay, out var intersectionDist);
            var place = pointRay.GetPoint(intersectionDist);
            var forwardOffset = GetWaveSpawnOffset(camera.transform.forward, plane.normal, waveRadius);
            var rightOffset = GetWaveSpawnOffset(camera.transform.right, plane.normal, waveRadius);
          
            place += spawnSide switch
            {
                SpawnSide.Top => forwardOffset,
                SpawnSide.Bottom => -forwardOffset,
                SpawnSide.Right => rightOffset,
                SpawnSide.Left => -rightOffset,
                _ => Vector3.zero
            };
            
            return place;
        }

        private Vector2 GetRandomPointOnViewportEdge(SpawnSide spawnSide)
        {
            switch (spawnSide)
            {
                case SpawnSide.Top:
                case SpawnSide.Bottom:
                    return new Vector2(Random.Range(0f, 1f), GetViewportEdge(spawnSide));
                case SpawnSide.Right:
                case SpawnSide.Left:
                    return new Vector2(GetViewportEdge(spawnSide), Random.Range(0f, 1f));
                default:
                    throw new ArgumentException("Unexpected spawn side");
            }
        }

        private float GetViewportEdge(SpawnSide spawnSide)
        {
            return (spawnSide == SpawnSide.Top || spawnSide == SpawnSide.Right) ? 1f : 0f;
        }

        private Vector3 GetWaveSpawnOffset(Vector3 vector, Vector3 normal, float waveRadius)
        {
            return Vector3.ProjectOnPlane(vector, normal) * (_minOutOfViewOffset + waveRadius);
        }

        private void SpawnEnemy(Vector3 place)
        {
            var enemy = _unitFactory.CreateEnemy();
            enemy.transform.position = place;
        }

        private void Dispose()
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
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

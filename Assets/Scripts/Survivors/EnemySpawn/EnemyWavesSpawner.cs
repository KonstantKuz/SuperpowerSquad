using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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

        private EnemyWavesConfig _wavesConfig;
        private Queue<EnemyWaveConfig> _currentMatchWaves;
        private EnemyWaveConfig _currentWave;
        private Coroutine _spawnCoroutine;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;

        public void StartSpawn(EnemyWavesConfig enemyWavesConfig)
        {
            _wavesConfig = enemyWavesConfig;
            _currentMatchWaves = new Queue<EnemyWaveConfig>(enemyWavesConfig.EnemySpawns);
            _currentWave = _currentMatchWaves.Dequeue();
            _spawnCoroutine = StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            while (true)
            {
                if (_currentMatchWaves.Count == 0)
                {
                    Dispose();
                    yield break;
                }

                var nextWave = _currentMatchWaves.Peek();
                var waitTime = _currentMatchWaves.Count == _wavesConfig.EnemySpawns.Count 
                    ? _currentWave.SpawnTime 
                    : nextWave.SpawnTime - _currentWave.SpawnTime;
                
                yield return new WaitForSeconds(waitTime);
                SpawnNextWave(_currentWave.Count);
            }
        }
        
        private void SpawnNextWave(int spawnCount)
        {
            var place = GetRandomPlaceForWave(spawnCount);
            for (int i = 0; i < spawnCount; i++) 
            {
                SpawnEnemy(place);
            }

            if (_currentMatchWaves.Count > 0) {
                _currentWave = _currentMatchWaves.Dequeue();
            }
        }
        
        private Vector3 GetRandomPlaceForWave(int waveCount)
        {
            var camera = UnityEngine.Camera.main;
            var randomViewportPoint = GetRandomPointOnViewportEdge(out var spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            var plane = new Plane(_world.Player.up, _world.Player.position);
            plane.Raycast(pointRay, out var intersectionDist);
            var place = pointRay.GetPoint(intersectionDist);
            var forwardOffset = Vector3.ProjectOnPlane(camera.transform.forward, plane.normal)
                                * (_minOutOfViewOffset + _outOfViewOffsetMultiplier * waveCount);
            var rightOffset = Vector3.ProjectOnPlane(camera.transform.right, plane.normal) 
                              * (_minOutOfViewOffset + _outOfViewOffsetMultiplier * waveCount);
          
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

        private Vector2 GetRandomPointOnViewportEdge(out SpawnSide spawnSide)
        {
            var placeAlongVertical = Random.value > 0.5f;
            var verticalValue = placeAlongVertical ? Random.Range(0f, 1f) : GetRandomViewportEdge();
            var horizontalValue = placeAlongVertical ? GetRandomViewportEdge() : Random.Range(0f, 1f);
            spawnSide = GetSpawnSide(placeAlongVertical, verticalValue, horizontalValue);
            return new Vector2(horizontalValue,verticalValue);
        }

        private float GetRandomViewportEdge()
        {
            return Random.value > 0.5f ? 1f : 0f;
        }

        private SpawnSide GetSpawnSide(bool placeAlongVertical, float verticalValue, float horizontalValue)
        {
            return placeAlongVertical 
                ? horizontalValue > 0.5f ? SpawnSide.Right : SpawnSide.Left 
                : verticalValue > 0.5f ? SpawnSide.Top : SpawnSide.Bottom;
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

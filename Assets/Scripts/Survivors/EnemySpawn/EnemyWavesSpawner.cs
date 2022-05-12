using System;
using System.Collections.Generic;
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
        [Range(0.1f, 0.3f)]
        [SerializeField] private float _outOfViewHorizontalOffset = 0.1f;
        [Range(0.01f, 0.05f)]
        [SerializeField] private float _outOfViewVerticalTopOffset = 0.01f;
        [Range(0.1f, 0.3f)]
        [SerializeField] private float _outOfViewVerticalBottomOffset = 0.1f;
        [SerializeField] private float _timerStep = 1f;
        
        private Queue<EnemyWaveConfig> _currentMatchWaves;
        private EnemyWaveConfig _currentWave;

        [Inject] private SessionService _sessionService;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private LocationWorld _locationWorld;

        private void Start()
        {
            _sessionService.Start();
        }

        public void Init(MatchEnemyWavesConfig enemyWavesConfig)
        {
            _currentMatchWaves = new Queue<EnemyWaveConfig>(enemyWavesConfig.EnemySpawns);
            _currentWave = _currentMatchWaves.Dequeue();
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_timerStep)).Subscribe(TrySpawnNextWave);
        }

        private void TrySpawnNextWave(long time)
        {
            if (time != _currentWave.Second) {
                return;
            }
            SpawnNextWave(_currentWave.Count);
        }

        private void SpawnNextWave(int spawnCount)
        {
            var place = GetRandomPlaceForWave();
            for (int i = 0; i < spawnCount; i++) {
                SpawnEnemy(place);
            }

            if (_currentMatchWaves.Count > 0) {
                _currentWave = _currentMatchWaves.Dequeue();
            }
        }

        private Vector3 GetRandomPlaceForWave()
        {
            var randomPointOutOfView = GetRandomPointOutOfViewport();
            var outOfViewRay =  UnityEngine.Camera.main.ViewportPointToRay(randomPointOutOfView);
            var plane = new Plane(_locationWorld.Player.up, _locationWorld.Player.position);
            plane.Raycast(outOfViewRay, out var intersectionDist);
            return outOfViewRay.GetPoint(intersectionDist);
        }

        private Vector2 GetRandomPointOutOfViewport()
        {
            var placeAlongVertical = GetRandomSign() > 0;
            var horizontalValue = GetHorizontalViewportValue(placeAlongVertical);
            var verticalValue = GetVerticalViewportValue(!placeAlongVertical);
            return new Vector2(horizontalValue,verticalValue);
        }

        private float GetRandomSign()
        {
            return Random.value > 0.5f ? 1 : -1;
        }

        private float GetHorizontalViewportValue(bool inBounds)
        {
            return inBounds ? Random.Range(0f, 1f) : 
                GetRandomSign() > 0 ? 1f + _outOfViewHorizontalOffset : -_outOfViewHorizontalOffset;
        }

        private float GetVerticalViewportValue(bool inBounds)
        {
            return inBounds ? Random.Range(0f, 1f) : 
                GetRandomSign() > 0 ? 1f + _outOfViewVerticalTopOffset : -_outOfViewVerticalBottomOffset;
        }

        private void SpawnEnemy(Vector3 place)
        {
            var enemy = _unitFactory.CreateEnemy();
            enemy.transform.position = place;
        }
    }
}

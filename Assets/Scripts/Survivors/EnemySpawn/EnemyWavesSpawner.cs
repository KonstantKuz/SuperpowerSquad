using System;
using System.Collections.Generic;
using Survivors.Session;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Location.EnemySpawn
{
    public class EnemyWavesSpawner : MonoBehaviour
    {
        [SerializeField] private float _timerStep = 1f;
        
        private Queue<EnemyWaveConfig> _currentMatchWaves;
        private EnemyWaveConfig _currentWave;

        [Inject] private UnitFactory _unitFactory;
        [Inject] private SessionService _sessionService;

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
            return Vector3.zero;
        }

        private void SpawnEnemy(Vector3 place)
        {
            var enemy = _unitFactory.CreateEnemy();
            enemy.transform.position = place;
        }
    }
}

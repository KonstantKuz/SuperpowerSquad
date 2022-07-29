using System;
using System.Linq;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Session.Service;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn
{
    public class WaveGroupsSpawner : MonoBehaviour
    {
        private LevelWavesConfig _currentLevelConfig;
        private int _currentWaveIndex;
        private CompositeDisposable _disposable;
        
        [Inject] private IMessenger _messenger;
        [Inject] private SessionService _sessionService;
        [Inject] private World _world;
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private GroupsSpawnerConfig _spawnerConfig;
     
        public int CurrentWaveKillCount { get; private set; }
        public int CurrentWaveCount => _currentLevelConfig.Waves[_currentWaveIndex].Count;
        
        public void StartSpawn(LevelWavesConfig levelConfig)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _currentLevelConfig = levelConfig;
            _currentWaveIndex = 0;
            _sessionService.Kills.Subscribe(UpdateCurrentWaveKillCount).AddTo(_disposable);

            SpawnCurrentWave();
            _messenger.Subscribe<WaveClearedMessage>(SpawnNextWave);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public void UpdateCurrentWaveKillCount(int globalKillCount)
        {
            CurrentWaveKillCount = globalKillCount - _currentLevelConfig.Waves.Take(_currentWaveIndex).Sum(it => it.Count);
        }

        public void SpawnNextWave(WaveClearedMessage msg)
        {
            if (_sessionService.SessionCompleted)
            {
                return;
            }
            
            _currentWaveIndex++;
            SpawnCurrentWave();
        }

        private void OnSessionFinished(SessionEndMessage msg)
        {
            _messenger.Unsubscribe<WaveClearedMessage>(SpawnNextWave);
        }
        
        private void SpawnCurrentWave()
        {
            var waveConfig = _currentLevelConfig.Waves[_currentWaveIndex];
            var enemiesLeftForSpawn = waveConfig.Count;
            var groupsCount = Random.Range(_spawnerConfig.MinGroupsCount, _spawnerConfig.MaxGroupsCount);
            var enemiesInGroup = enemiesLeftForSpawn / groupsCount;
            for (;enemiesLeftForSpawn > 0; enemiesLeftForSpawn -= enemiesInGroup)
            {
                SpawnGroup(Mathf.Min(enemiesInGroup, enemiesLeftForSpawn), waveConfig);
            }
        }

        private void SpawnGroup(int enemiesInGroup, EnemyWaveConfig waveConfig)
        {
            var groupConfig = new EnemyWaveConfig
            {
                Count =  enemiesInGroup, 
                EnemyId = waveConfig.EnemyId,
                EnemyLevel = waveConfig.EnemyLevel
            };
            var spawnPlace = FindSpawnPlace(groupConfig);
            _enemyWavesSpawner.SpawnWave(groupConfig, spawnPlace);
        }

        private SpawnPlace FindSpawnPlace(EnemyWaveConfig groupConfig)
        {
            var spawnPlace = new SpawnPlace();
            var groundBounds = _world.Ground.GetComponent<Collider>().bounds;
            var x = Random.Range(-groundBounds.extents.x, groundBounds.extents.x);
            var z = Random.Range(-groundBounds.extents.z, groundBounds.extents.z);
            spawnPlace.Position = new Vector3(x, 0, z);
            spawnPlace.IsValid = true;
            spawnPlace = SnapPlaceToNavMesh(spawnPlace);

            var groupRadius = _enemyWavesSpawner.GetWaveRadius(groupConfig);
            var directionToPlayer = (_world.Squad.Destination.transform.position - spawnPlace.Position).normalized;
            var nearestToPlayerEnemyPosition = spawnPlace.Position + directionToPlayer * groupRadius;

            if (IsInsideCameraView(nearestToPlayerEnemyPosition))
            {
                return FindSpawnPlace(groupConfig);
            }

            return spawnPlace;
        }

        private SpawnPlace SnapPlaceToNavMesh(SpawnPlace spawnPlace)
        {
            NavMesh.SamplePosition(spawnPlace.Position, out var hit, Mathf.Infinity, NavMesh.AllAreas); 
            spawnPlace.Position = hit.position; 
            return spawnPlace;
        }

        private bool IsInsideCameraView(Vector3 position)
        {
            var screenPoint = UnityEngine.Camera.main.WorldToScreenPoint(position);
            return screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height;
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}

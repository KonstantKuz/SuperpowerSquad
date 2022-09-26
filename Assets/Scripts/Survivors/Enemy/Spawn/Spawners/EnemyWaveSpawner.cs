using System;
using System.Collections.Generic;
using Feofun.Config;
using Logger.Extension;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class EnemyWaveSpawner : MonoBehaviour
    {
        private const string ENEMY_LAYER_NAME = "Enemy";
        private static int ENEMY_LAYER;
        
        [SerializeField] private int _angleAttemptCount = 3;
        [SerializeField] private int _rangeAttemptCount = 3;
        [SerializeField] private float _minOutOfViewOffset = 2f;
        [SerializeField] private float _insideViewOffset = 5f;
        [SerializeField] private float _onEdgeViewOffset = 0.1f;

        [Inject] private World _world;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        
        private SpawnerDebugger _spawnerDebugger;
        
        private SpawnerDebugger Debugger => _spawnerDebugger ??= gameObject.AddComponent<SpawnerDebugger>();
        private void Awake()
        {
            ENEMY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
        }

        public void SpawnWave(EnemyWaveConfig wave, ISpawnPlaceProvider placeProvider)
        {
            var place = GetPlaceForWave(wave, placeProvider);
            SpawnWave(wave, place);
        }

        public void SpawnWave(EnemyWaveConfig wave, SpawnPlace spawnPlace)
        {
            if (!spawnPlace.IsValid) {
                this.Logger().Warn("Invalid spawn place provided. Spawn wave has been canceled.");
                return;
            }

            for (int i = 0; i < wave.Count; i++) {
                SpawnEnemy(spawnPlace.Position, wave);
            }
        }

        public SpawnPlace GetPlaceForWave(EnemyWaveConfig waveConfig, ISpawnPlaceProvider placeProvider) => FindEmptyPlace(waveConfig, placeProvider);
        
        public bool IsPlaceValid(Vector3 place, EnemyWaveConfig waveConfig)
        {
            return IsPlaceOnNavMesh(place) && !IsPlaceBusy(place, waveConfig);
        }
        private SpawnPlace FindEmptyPlace(EnemyWaveConfig waveConfig, ISpawnPlaceProvider placeProvider)
        {
            for (int rangeTry = 1; rangeTry <= _rangeAttemptCount; rangeTry++) {
                var outOfViewOffset = GetOutOfViewOffset(waveConfig, rangeTry);
                for (int angleTry = 0; angleTry < _angleAttemptCount; angleTry++) {
                    var spawnPlace = placeProvider.GetSpawnPlace(waveConfig, outOfViewOffset);
                    if (spawnPlace.IsValid) {
                        return spawnPlace;
                    }
                }
            }

            return SpawnPlace.INVALID;
        }

        private float GetOutOfViewOffset(EnemyWaveConfig waveConfig, int rangeTry)
        {
            return waveConfig.PlacingType switch {
                    WavePlacingType.OutsideView => _minOutOfViewOffset + rangeTry * GetWaveRadius(waveConfig),
                    WavePlacingType.InsideView => -_insideViewOffset,
                    WavePlacingType.OnViewEdge => _onEdgeViewOffset,
                    _ => throw new ArgumentOutOfRangeException()
            };
        }

        private float GetWaveRadius(EnemyWaveConfig waveConfig)
        {
            var enemyConfig = _enemyUnitConfigs.Get(waveConfig.EnemyId);
            return Mathf.Sqrt(waveConfig.Count) * enemyConfig.CalculateScale(waveConfig.EnemyLevel);
        }

 

        private bool IsPlaceOnNavMesh(Vector3 place)
        {
            return NavMesh.SamplePosition(place, out var hit, 1f, NavMesh.AllAreas);
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
            enemyAi.NavMeshAgent.Warp(place + Vector3.up);
            enemy.transform.LookAt(_world.Squad.Position);
        }
    }
}
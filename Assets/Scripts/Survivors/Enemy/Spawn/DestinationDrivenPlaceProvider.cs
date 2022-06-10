using System;
using System.Linq;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public class DestinationDrivenPlaceProvider : IEnemySpawnPlaceProvider
    {
        private const int CAMERA_MAX_FRUSTUM_PLANES_COUNT = 6;
        private const int CAMERA_SIDE_FRUSTUM_PLANES_COUNT = 4;
        private readonly Plane[] _frustumPlanes = new Plane[CAMERA_MAX_FRUSTUM_PLANES_COUNT];
        
        private readonly EnemyWavesSpawner _wavesSpawner;
        private readonly Squad.Squad _squad;

        public DestinationDrivenPlaceProvider(EnemyWavesSpawner wavesSpawner, Squad.Squad squad)
        {
            _wavesSpawner = wavesSpawner;
            _squad = squad;
        }

        public Vector3 GetSpawnPlace(EnemyWaveConfig waveConfig, int spawnOffsetMultiplier)
        {
            var spawnPlace = GetSpawnPlaceByDestination(waveConfig, spawnOffsetMultiplier);
            return spawnPlace == EnemyWavesSpawner.INVALID_SPAWN_PLACE ? EnemyWavesSpawner.INVALID_SPAWN_PLACE : spawnPlace;
        }
        
        private Vector3 GetSpawnPlaceByDestination(EnemyWaveConfig waveConfig, int spawnOffsetMultiplier)
        {
            var moveDirection = _squad.MoveDirection.normalized;
  
            if (Math.Abs(moveDirection.magnitude) < Mathf.Epsilon)
            {
                return EnemyWavesSpawner.INVALID_SPAWN_PLACE;
            }
            
            var ray = new Ray(_squad.Destination.transform.position, moveDirection);
            var outOfViewPlaceOnGround = EnemyWavesSpawner.INVALID_SPAWN_PLACE;

            var camera = UnityEngine.Camera.main;
            GeometryUtility.CalculateFrustumPlanes(camera, _frustumPlanes);
            foreach (var plane in _frustumPlanes.Take(CAMERA_SIDE_FRUSTUM_PLANES_COUNT))
            {
                if (plane.Raycast(ray, out var distance)) 
                    outOfViewPlaceOnGround = ray.GetPoint(distance);
            }

            var spawnOffset = _wavesSpawner.GetSpawnOffset(waveConfig) * spawnOffsetMultiplier;
            return outOfViewPlaceOnGround + moveDirection * spawnOffset;
        }
    }
}
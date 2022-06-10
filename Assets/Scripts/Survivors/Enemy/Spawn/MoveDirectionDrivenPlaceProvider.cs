using System;
using System.Linq;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public class MoveDirectionDrivenPlaceProvider : ISpawnPlaceProvider
    {
        private const int CAMERA_MAX_FRUSTUM_PLANES_COUNT = 6;
        private const int CAMERA_SIDE_FRUSTUM_PLANES_COUNT = 4;
        private readonly Plane[] _frustumPlanes = new Plane[CAMERA_MAX_FRUSTUM_PLANES_COUNT];
        
        private readonly EnemyWavesSpawner _wavesSpawner;
        private readonly Squad.Squad _squad;

        public MoveDirectionDrivenPlaceProvider(EnemyWavesSpawner wavesSpawner, Squad.Squad squad)
        {
            _wavesSpawner = wavesSpawner;
            _squad = squad;
        }

        public Vector3 GetSpawnPlace(EnemyWaveConfig waveConfig, int outOfViewMultiplier)
        {
            var spawnPlace = GetSpawnPlaceByDestination(waveConfig, outOfViewMultiplier);
            return spawnPlace == EnemyWavesSpawner.INVALID_SPAWN_PLACE ? EnemyWavesSpawner.INVALID_SPAWN_PLACE : spawnPlace;
        }
        
        private Vector3 GetSpawnPlaceByDestination(EnemyWaveConfig waveConfig, int outOfViewMultiplier)
        {
            var destination = _squad.MoveDirection.normalized;
  
            if (Math.Abs(destination.magnitude) < Mathf.Epsilon)
            {
                return EnemyWavesSpawner.INVALID_SPAWN_PLACE;
            }
            
            var ray = new Ray(_squad.Destination.transform.position, destination);
            var frustumIntersectionPlace = EnemyWavesSpawner.INVALID_SPAWN_PLACE;

            var camera = UnityEngine.Camera.main;
            GeometryUtility.CalculateFrustumPlanes(camera, _frustumPlanes);
            foreach (var plane in _frustumPlanes.Take(CAMERA_SIDE_FRUSTUM_PLANES_COUNT))
            {
                if (plane.Raycast(ray, out var distance)) 
                    frustumIntersectionPlace = ray.GetPoint(distance);
            }

            var outOfViewOffset = _wavesSpawner.GetOutOfViewOffset(waveConfig, outOfViewMultiplier);
            return frustumIntersectionPlace + destination * outOfViewOffset;
        }
    }
}
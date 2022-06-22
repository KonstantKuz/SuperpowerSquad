using System;
using System.Linq;
using Survivors.Enemy.Spawn.Config;
using Survivors.Extension;
using UnityEngine;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class MoveDirectionDrivenPlaceProvider : ISpawnPlaceProvider
    {
        private const int MAX_FRUSTUM_PLANES_COUNT = 6;
        private const int VIEW_FRUSTUM_PLANES_COUNT = 4;
        private readonly Plane[] _frustumPlanes = new Plane[MAX_FRUSTUM_PLANES_COUNT];
        
        private readonly EnemyWavesSpawner _wavesSpawner;
        private readonly Squad.Squad _squad;

        public MoveDirectionDrivenPlaceProvider(EnemyWavesSpawner wavesSpawner, Squad.Squad squad)
        {
            _wavesSpawner = wavesSpawner;
            _squad = squad;
        }

        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry)
        {
            var moveDirection = _squad.MoveDirection.XZ();
            if (moveDirection.magnitude < Mathf.Epsilon)
            {
                return SpawnPlace.INVALID;
            }
            var position = GetSpawnPlaceByDestination(waveConfig, rangeTry, moveDirection);
            if (position == null)
            {
                return SpawnPlace.INVALID;
            }
            var isValid = _wavesSpawner.IsPlaceValid(position.Value, waveConfig);
            return new SpawnPlace {IsValid = isValid, Position = position.Value};
        }
        
        private Vector3? GetSpawnPlaceByDestination(EnemyWaveConfig waveConfig, int rangeTry, Vector3 moveDirection)
        {
            var ray = new Ray(_squad.Destination.transform.position, moveDirection);
            var frustumIntersectionPoint = GetFrustumIntersectionPoint(ray);

            if (frustumIntersectionPoint == null)
            {
                return null;
            }
            
            var outOfViewOffset = _wavesSpawner.GetOutOfViewOffset(waveConfig, rangeTry);
            return frustumIntersectionPoint + moveDirection * outOfViewOffset;
        }

        private Vector3? GetFrustumIntersectionPoint(Ray ray)
        {
            var camera = UnityEngine.Camera.main;
            GeometryUtility.CalculateFrustumPlanes(camera, _frustumPlanes);
            foreach (var plane in _frustumPlanes.Take(VIEW_FRUSTUM_PLANES_COUNT))
            {
                if (plane.Raycast(ray, out var distance)) 
                    return ray.GetPoint(distance);
            }

            return null;
        }
    }
}
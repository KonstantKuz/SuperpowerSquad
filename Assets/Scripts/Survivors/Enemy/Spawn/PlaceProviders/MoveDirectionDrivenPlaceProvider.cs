using System;
using System.Linq;
using Survivors.Enemy.Spawn.Config;
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
            if (!_squad.IsMoving)
            {
                return SpawnPlace.INVALID;
            }

            var position = GetSpawnPlaceByDestination(waveConfig, rangeTry);
            var isValid = !_wavesSpawner.IsPlaceBusy(position, waveConfig);
            return new SpawnPlace {IsValid = isValid, Position = position};
        }
        
        private Vector3 GetSpawnPlaceByDestination(EnemyWaveConfig waveConfig, int rangeTry)
        {
            var destination = _squad.MoveDirection.normalized;
            var ray = new Ray(_squad.Destination.transform.position, destination);
            var frustumIntersectionPoint = GetFrustumIntersectionPoint(ray);
            var outOfViewOffset = _wavesSpawner.GetOutOfViewOffset(waveConfig, rangeTry);
            return frustumIntersectionPoint + destination * outOfViewOffset;
        }

        private Vector3 GetFrustumIntersectionPoint(Ray ray)
        {
            var camera = UnityEngine.Camera.main;
            GeometryUtility.CalculateFrustumPlanes(camera, _frustumPlanes);
            foreach (var plane in _frustumPlanes.Take(VIEW_FRUSTUM_PLANES_COUNT))
            {
                if (plane.Raycast(ray, out var distance)) 
                    return ray.GetPoint(distance);
            }

            throw new Exception("Ray must intersect one of the camera frustum planes.");
        }
    }
}
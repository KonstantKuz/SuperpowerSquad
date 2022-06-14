using System;
using Feofun.Extension;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class RandomDrivenPlaceProvider : ISpawnPlaceProvider
    {
        private readonly EnemyWavesSpawner _wavesSpawner;
        private readonly World _world;

        public RandomDrivenPlaceProvider(EnemyWavesSpawner wavesSpawner, World world)
        {
            _wavesSpawner = wavesSpawner;
            _world = world;
        }

        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry)
        {
            var position = GetRandomSpawnPosition(waveConfig, rangeTry);
            var isValid = !_wavesSpawner.IsPlaceBusy(position, waveConfig);
            return new SpawnPlace {IsValid = isValid, Position = position};
        }

        private Vector3 GetRandomSpawnPosition(EnemyWaveConfig waveConfig, int rangeTry)
        {
            var outOfViewOffset = _wavesSpawner.GetOutOfViewOffset(waveConfig, rangeTry);
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            var randomPosition = GetRandomPositionOnGround(spawnSide);
            return GetPositionWithOffset(randomPosition, spawnSide, outOfViewOffset);
        }

        private Vector3 GetRandomPositionOnGround(SpawnSide spawnSide)
        {
            var camera = UnityEngine.Camera.main;
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            return _world.GetGroundIntersection(pointRay);
        }

        private Vector3 GetPositionWithOffset(Vector3 position, SpawnSide spawnSide, float outOfViewOffset)
        {
            var camera = UnityEngine.Camera.main.transform;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.forward, _world.Ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.right, _world.Ground.up).normalized;
            position += spawnSide switch
            {
                SpawnSide.Top => directionToTopSide * outOfViewOffset,
                SpawnSide.Bottom => -directionToTopSide * outOfViewOffset,
                SpawnSide.Right => directionToRightSide * outOfViewOffset,
                SpawnSide.Left => -directionToRightSide * outOfViewOffset,
                _ => Vector3.zero
            };
            return position;
        }

        private Vector2 GetRandomPointOnViewportEdge(SpawnSide spawnSide)
        {
            switch (spawnSide)
            {
                case SpawnSide.Top:
                    return new Vector2(Random.Range(0f, 1f), 1f);
                case SpawnSide.Bottom:
                    return new Vector2(Random.Range(0f, 1f), 0f);
                case SpawnSide.Right:
                    return new Vector2(1f, Random.Range(0f, 1f));
                case SpawnSide.Left:
                    return new Vector2(0f, Random.Range(0f, 1f));
                default:
                    throw new ArgumentException("Unexpected spawn side");
            }
        }

    }
}
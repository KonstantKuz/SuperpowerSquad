using System;
using Feofun.Extension;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class RandomSideDrivenPlaceProvider : ISpawnPlaceProvider
    {
        private readonly EnemyWaveSpawner _spawner;
        private readonly World _world;

        public RandomSideDrivenPlaceProvider(EnemyWaveSpawner spawner, World world)
        {
            _spawner = spawner;
            _world = world;
        }

        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, float outOfViewOffset)
        {
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            var position = GetRandomSpawnPosition(spawnSide, outOfViewOffset);
            var isValid = _spawner.IsPlaceValid(position, waveConfig);
            return new SpawnPlace {
                    IsValid = isValid,
                    Position = position
            };
        }

        public static Vector3 GetPositionWithOffset(Vector3 position, SpawnSide spawnSide, float outOfViewOffset, Transform ground)
        {
            var camera = UnityEngine.Camera.main.transform;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.forward, ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.right, ground.up).normalized;
            position += spawnSide switch {
                    SpawnSide.Top => directionToTopSide * outOfViewOffset,
                    SpawnSide.Bottom => -directionToTopSide * outOfViewOffset,
                    SpawnSide.Right => directionToRightSide * outOfViewOffset,
                    SpawnSide.Left => -directionToRightSide * outOfViewOffset,
                    _ => Vector3.zero
            };
            return position;
        }

        public Vector3 GetRandomSpawnPosition(SpawnSide spawnSide, float outOfViewOffset)
        {
            var randomPosition = GetRandomPositionOnGround(spawnSide);
            return GetPositionWithOffset(randomPosition, spawnSide, outOfViewOffset, _world.Ground);
        }

        private Vector3 GetRandomPositionOnGround(SpawnSide spawnSide)
        {
            var camera = UnityEngine.Camera.main;
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay = camera.ViewportPointToRay(randomViewportPoint);
            return _world.GetGroundIntersection(pointRay);
        }

        private Vector2 GetRandomPointOnViewportEdge(SpawnSide spawnSide)
        {
            switch (spawnSide) {
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
using System;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class SideDrivenPlaceProvider : ISpawnPlaceProvider
    {
        private readonly EnemyWaveSpawner _spawner;
        private readonly World _world;
        private readonly SpawnSide _spawnSide;

        public SideDrivenPlaceProvider(EnemyWaveSpawner spawner, World world, SpawnSide spawnSide)
        {
            _spawner = spawner;
            _spawnSide = spawnSide;
            _world = world;
        }

        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, float outOfViewOffset)
        {
            var positionOnGround = GetPositionOnGround(_spawnSide);
            var positionWithOffset = RandomSideDrivenPlaceProvider.GetPositionWithOffset(positionOnGround, _spawnSide, outOfViewOffset, _world.Ground);
            var isValid = _spawner.IsPlaceValid(positionWithOffset, waveConfig);
            return new SpawnPlace {
                    IsValid = isValid,
                    Position = positionWithOffset
            };
        }

        private Vector3 GetPositionOnGround(SpawnSide spawnSide)
        {
            var camera = UnityEngine.Camera.main;
            var viewportPoint = GetPointOnViewportEdge(spawnSide);
            var pointRay = camera.ViewportPointToRay(viewportPoint);
            return _world.GetGroundIntersection(pointRay);
        }

        private Vector2 GetPointOnViewportEdge(SpawnSide spawnSide)
        {
            switch (spawnSide) {
                case SpawnSide.Top:
                    return new Vector2(0.5f, 1f);
                case SpawnSide.Bottom:
                    return new Vector2(0.5f, 0f);
                case SpawnSide.Right:
                    return new Vector2(1f, 0.5f);
                case SpawnSide.Left:
                    return new Vector2(0f, 0.5f);
                default:
                    throw new ArgumentException("Unexpected spawn side");
            }
        }

  
    }
}
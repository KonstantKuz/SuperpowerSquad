﻿using System;
using Feofun.Extension;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn
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

        public Vector3 GetSpawnPlace(EnemyWaveConfig waveConfig, int outOfViewMultiplier)
        {
            var spawnPlace = GetRandomSpawnPlace(waveConfig, outOfViewMultiplier);
            return spawnPlace == EnemyWavesSpawner.INVALID_SPAWN_PLACE ? EnemyWavesSpawner.INVALID_SPAWN_PLACE : spawnPlace;
        }

        private Vector3 GetRandomSpawnPlace(EnemyWaveConfig waveConfig, int outOfViewMultiplier)
        {
            var outOfViewOffset = _wavesSpawner.GetOutOfViewOffset(waveConfig, outOfViewMultiplier);
            var spawnSide = EnumExt.GetRandom<SpawnSide>();
            return GetRandomPlaceOnGround(spawnSide, outOfViewOffset);
        }
        
        private Vector3 GetRandomPlaceOnGround(SpawnSide spawnSide, float outOfViewOffset)
        {
            var camera = UnityEngine.Camera.main.transform;
            var directionToTopSide = Vector3.ProjectOnPlane(camera.forward, _world.Ground.up).normalized;
            var directionToRightSide = Vector3.ProjectOnPlane(camera.right, _world.Ground.up).normalized;

            var randomPlace = GetRandomPlaceOnGround(spawnSide);
            randomPlace += spawnSide switch
            {
                SpawnSide.Top => directionToTopSide * outOfViewOffset,
                SpawnSide.Bottom => -directionToTopSide * outOfViewOffset,
                SpawnSide.Right => directionToRightSide * outOfViewOffset,
                SpawnSide.Left => -directionToRightSide * outOfViewOffset,
                _ => Vector3.zero
            };
            return randomPlace;
        }

        private Vector3 GetRandomPlaceOnGround(SpawnSide spawnSide)
        {
            var camera = UnityEngine.Camera.main;
            var randomViewportPoint = GetRandomPointOnViewportEdge(spawnSide);
            var pointRay =  camera.ViewportPointToRay(randomViewportPoint);
            return _world.GetGroundIntersection(pointRay);
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
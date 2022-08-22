using System;
using System.Collections.Generic;
using Survivors.Location;
using Survivors.WorldEvents.Events.Lava.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Lava
{
    public class CircleLavaSpawner
    {
        private readonly LavaEventConfig _config;
        private readonly World _world;

        private float MaxSpawnDistance => _world.GetSquad().Model.Speed.Value * _config.EventDuration;

        public CircleLavaSpawner(LavaEventConfig config, World world)
        {
            _config = config;
            _world = world;
        }

        public void SpawnLava(Action<Vector3> onCreateLava)
        {
            for (int spawnDistance = (int) _config.MinLavaDiameter; spawnDistance < MaxSpawnDistance; spawnDistance += (int) (_config.MinLavaDiameter * 2)) {
                var spawnCount = CalculateSpawnCount(spawnDistance);
                foreach (var place in GetSpawnPlacesOnCircle(_world.GetSquad().Position, spawnDistance, spawnCount)) {
                    onCreateLava(place);
                }
            }
        }

        private int CalculateSpawnCount(float spawnRadius)
        {
            var perimeter = (2 * Mathf.PI * spawnRadius);
            return (int) (perimeter / _config.SpawnStepOnPerimeter);
        }

        private IEnumerable<Vector3> GetSpawnPlacesOnCircle(Vector3 circleCenter, float spawnDistance, int spawnObjectCount)
        {
            var maxAngle = Mathf.Rad2Deg * (2 * Mathf.PI);
            var stepAngle = maxAngle / spawnObjectCount;
            for (float angle = stepAngle; angle <= maxAngle; angle += stepAngle) {
                yield return CalculateRandomPointOnSection(circleCenter, angle, stepAngle, spawnDistance);
            }
        }

        private Vector3 CalculateRandomPointOnSection(Vector3 circleCenter, float angle, float stepAngle, float spawnDistance)
        {
            var randomAngle = Random.Range(angle - stepAngle / 2, angle);
            var randomSpawnDistance = Random.Range(spawnDistance - _config.MinLavaDiameter, spawnDistance + _config.MinLavaDiameter);
            return circleCenter + Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * randomSpawnDistance;
        }
    }
}
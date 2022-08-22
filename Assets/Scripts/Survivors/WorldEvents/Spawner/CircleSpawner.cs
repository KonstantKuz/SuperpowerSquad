using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Spawner
{
    public class CircleSpawner
    {
        private readonly ICircleSpawnParams _config;

        public CircleSpawner(ICircleSpawnParams config)
        {
            _config = config;
        }
        public void Spawn(Vector3 spawnCircleCenter, Action<Vector3> onCreate)
        {
            for (float spawnDistance = _config.InitialSpawnDistance; spawnDistance < _config.MaxSpawnDistance; spawnDistance += _config.SpawnDistanceStep) {
                var spawnCount = CalculateSpawnCount(spawnDistance);
                foreach (var place in GetSpawnPlacesOnCircle(spawnCircleCenter, spawnDistance, spawnCount)) {
                    onCreate(place);
                }
            }
        }
        private int CalculateSpawnCount(float spawnRadius)
        {
            var circlePerimeter = (2 * Mathf.PI * spawnRadius);
            return (int) (circlePerimeter / _config.SpawnStepOnPerimeter);
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
            var randomSpawnDistance = Random.Range(spawnDistance - _config.SpawnDistanceStep / 2, spawnDistance + _config.SpawnDistanceStep / 2);
            return circleCenter + Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * randomSpawnDistance;
        }
    }
}
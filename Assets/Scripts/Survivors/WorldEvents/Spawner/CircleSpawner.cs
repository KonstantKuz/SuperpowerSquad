using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Spawner
{
    public class CircleSpawner
    {
        private const int MAX_SPAWN_ANGLE = 360;

        private readonly ICircleSpawnParams _config;

        public CircleSpawner(ICircleSpawnParams config)
        {
            _config = config;
        }
        public void Spawn(Vector3 spawnCircleCenter, Action<Vector3> onCreateLava)
        {
            var spawnCountOnCircle = _config.InitialSpawnCountOnCircle;
            
            for (float spawnDistance = _config.InitialSpawnDistance; spawnDistance < _config.MaxSpawnDistance; spawnDistance += _config.SpawnDistanceStep) {
                foreach (var place in GetSpawnPlacesOnCircle(spawnCircleCenter, spawnDistance, spawnCountOnCircle)) {
                    onCreateLava(place);
                }
                spawnCountOnCircle += _config.SpawnCountIncrementStepOnCircle;
            }
        }
        private IEnumerable<Vector3> GetSpawnPlacesOnCircle(Vector3 circleCenter, float spawnDistance, int spawnObjectCount)
        {
            var stepAngle = MAX_SPAWN_ANGLE / spawnObjectCount;
            for (int angle = stepAngle; angle <= MAX_SPAWN_ANGLE; angle += stepAngle) {
                yield return CalculateRandomPointOnCircle(circleCenter, angle, stepAngle, spawnDistance);
            }
        }
        private Vector3 CalculateRandomPointOnCircle(Vector3 circleCenter, float angle, float stepAngle, float spawnDistance)
        {
            var randomAngle = Random.Range(angle - stepAngle / 2, angle);
            var randomSpawnDistance = Random.Range(spawnDistance - _config.SpawnDistanceStep / 2, spawnDistance + _config.SpawnDistanceStep / 2);
            return circleCenter + GetPointOnCircle(randomAngle) * randomSpawnDistance;
        }

        private Vector3 GetPointOnCircle(float angle)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
        }
    }
}
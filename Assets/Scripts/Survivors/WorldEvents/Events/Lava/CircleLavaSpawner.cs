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
        private const int MAX_SPAWN_ANGLE = 360;

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
            var spawnCountOnCircle = _config.SpawnCountOnCircle;
            
            for (int spawnDistance = (int) _config.MinLavaDiameter; spawnDistance < MaxSpawnDistance; spawnDistance += (int) (_config.MinLavaDiameter * 2)) {
                foreach (var place in GetSpawnPlacesOnCircle(_world.GetSquad().Position, spawnDistance, spawnCountOnCircle)) {
                    onCreateLava(place);
                }
                spawnCountOnCircle += _config.SpawnCountStepOnCircle;
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
            var randomSpawnDistance = Random.Range(spawnDistance - _config.MinLavaDiameter, spawnDistance + _config.MinLavaDiameter);
            return circleCenter + GetPointOnCircle(randomAngle) * randomSpawnDistance;
        }

        private Vector3 GetPointOnCircle(float angle)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
        }
    }
}
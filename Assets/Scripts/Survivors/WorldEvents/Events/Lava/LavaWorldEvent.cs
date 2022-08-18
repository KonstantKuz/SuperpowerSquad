using System.Collections;
using System.Collections.Generic;
using Logger.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Lava
{
    public class LavaWorldEvent : WorldEvent
    {
        private const int MAX_SPAWN_ANGLE = 360;
        
        private readonly List<Lava> _createdLava = new List<Lava>();

        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;

        private LavaEventConfig _config;
        
        private float MaxSpawnDistance => _world.GetSquad().Model.Speed.Value * _config.EventDuration;
        
        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("LavaWorldEvent started");
            _config = (LavaEventConfig) config;

            SpawnLava();
            yield return WaitFinish(_config);
        }

        private void SpawnLava()
        {
            var count = 5;
            var step = 6;
            for (int spawnDistance = (int) _config.MinLavaDiameter; spawnDistance < MaxSpawnDistance; spawnDistance += (int) (_config.MinLavaRadius * 4)) {
                
                foreach (var place in GetSpawnPlaces(_world.Squad.Position, spawnDistance, count)) {
                    SpawnLava(place);
                }
                count += step;
            }
        }
      
        
        private IEnumerator WaitFinish(LavaEventConfig config)
        {
            yield return new WaitForSeconds(config.EventDuration);
            DisposeLava();
            this.Logger().Trace("LavaWorldEvent finished");
        }

        private void SpawnLava(Vector3 place)
        {
            var lava = _worldObjectFactory.CreateObject<Lava>("Lava");
            lava.transform.SetPositionAndRotation(place, Quaternion.identity);
            lava.Init(_config);
            _createdLava.Add(lava);
        }
        private IEnumerable<Vector3> GetSpawnPlaces(Vector3 center, float spawnDistance, int count)
        {
            var stepAngle = MAX_SPAWN_ANGLE / count;
            
            for (int angle = stepAngle; angle <= MAX_SPAWN_ANGLE; angle += stepAngle ) {
                var finalAngle = Random.Range(angle - stepAngle / 2, angle); 
                var point = center + GetPointOnCircle(finalAngle) * Random.Range(spawnDistance - _config.MinLavaDiameter, spawnDistance + _config.MinLavaDiameter);
                yield return point;
            }
        }

        private Vector3 GetPointOnCircle(float angle)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
        }
        private void DisposeLava()
        {
            _createdLava.ForEach(it => { it.Dispose(); });
            _createdLava.Clear();
        }
    }
}
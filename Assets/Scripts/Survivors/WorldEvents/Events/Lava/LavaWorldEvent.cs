using System;
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
        private const int SEARCH_POSITION_ANGLE_MAX = 360;
        
        private readonly List<Lava> _lava = new List<Lava>();

        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;

        private LavaEventConfig _config;

        private Coroutine _spawnCoroutine;

        public override event Action OnFinished;

        public override void Start(EventConfig config)
        {
            this.Logger().Trace("LavaWorldEvent started");
            _config = (LavaEventConfig) config;

            SpawnLava();

            GameApplication.Instance.StartCoroutine(WaitFinish(_config));
        }

        private void SpawnLava()
        {
            var spawnDistance = _world.Squad.Model.Speed.Value * _config.EventDuration;
            var count = 4;
            var step = 7;
            for (int outOfViewOffset = (int) _config.Radius * 3; outOfViewOffset < spawnDistance; outOfViewOffset += (int) (_config.Radius * 4)) {
                
                foreach (var place in GetSpawnPlaces(_world.Squad.Position, outOfViewOffset, count)) {
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
            OnFinished?.Invoke();
        }

        private void SpawnLava(Vector3 place)
        {
            var lava = _worldObjectFactory.CreateObject<Lava>("Lava");
            lava.transform.SetPositionAndRotation(place, Quaternion.identity);
            lava.Init(_config);
            _lava.Add(lava);
        }
        private IEnumerable<Vector3> GetSpawnPlaces(Vector3 center, float range, int count)
        {
            var stepAngle = SEARCH_POSITION_ANGLE_MAX / count;
            
            for (int angle = stepAngle; angle <= SEARCH_POSITION_ANGLE_MAX; angle += stepAngle ) {
                var finalAngle = Random.Range(angle - stepAngle / 2, angle); 
                var point = center + GetPointOnCircle(finalAngle) * Random.Range(range - _config.Radius, range + _config.Radius);
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
            _lava.ForEach(it => { it.Dispose(); });
            _lava.Clear();
        }
    }
}
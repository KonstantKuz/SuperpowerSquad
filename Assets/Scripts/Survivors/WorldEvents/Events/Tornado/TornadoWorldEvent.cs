using System.Collections;
using System.Collections.Generic;
using Logger.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.WorldEvents.Events.Tornado.Config;
using Survivors.WorldEvents.Spawner;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Tornado
{
    public class TornadoWorldEvent : WorldEvent
    {
        private readonly List<Tornado> _createdTornado = new List<Tornado>();

        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;

        private TornadoEventConfig _config;

        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("TornadoWorldEvent started");
            _config = (TornadoEventConfig) config;
       
            var spawnParams = _config.SpawnParams;
            spawnParams.MaxSpawnDistance = _world.GetSquad().Model.Speed.Value * _config.EventDuration;
            
            var spawner = new CircleSpawner(spawnParams);
            spawner.Spawn(_world.GetSquad().Position, CreateTornado);
          //  CreateTornado(Vector3.zero);
            yield return WaitFinish(_config);
        }

        private void CreateTornado(Vector3 place)
        {
            var tornado = _worldObjectFactory.CreateObject<Tornado>(_config.PrefabId);
            tornado.transform.SetPositionAndRotation(place, Quaternion.identity);
            tornado.Init(_config);
            _createdTornado.Add(tornado);
        }

        protected override void Term()
        {
            _createdTornado.ForEach(it => { it.Term(); });
            _createdTornado.Clear();
        }
    }
}
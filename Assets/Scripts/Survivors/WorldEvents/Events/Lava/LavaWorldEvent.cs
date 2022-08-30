using System.Collections;
using System.Collections.Generic;
using Logger.Extension;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.WorldEvents.Events.Lava.Config;
using Survivors.WorldEvents.Spawner;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Lava
{
    public class LavaWorldEvent : WorldEvent
    {
        private readonly List<Lava> _createdLava = new List<Lava>();

        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectFactory;  
        [Inject]
        private World _world;

        private LavaEventConfig _config;

        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("LavaWorldEvent started");
            
            _config = (LavaEventConfig) config;
            var spawnParams = _config.SpawnParams;
            spawnParams.MaxSpawnDistance = _world.GetSquad().Model.Speed.Value * _config.EventDuration;
            
            var spawner = new CircleSpawner(spawnParams);
            spawner.Spawn(_world.GetSquad().Position, CreateLava);
            yield return WaitFinish(_config);
        }

        private void CreateLava(Vector3 place)
        {
            var lava = _objectFactory.Create<Lava>(_config.LavaPrefabId);
            lava.transform.SetPositionAndRotation(place, Quaternion.identity);
            lava.Init(_config);
            _createdLava.Add(lava);
        }

        protected override void Term()
        {
            _createdLava.ForEach(it => { it.Term(); });
            _createdLava.Clear();
        }
    }
}
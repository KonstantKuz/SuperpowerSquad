using System.Collections;
using System.Collections.Generic;
using Logger.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.WorldEvents.Events.Lava.Config;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Lava
{
    public class LavaWorldEvent : WorldEvent
    {
        private readonly List<Lava> _createdLava = new List<Lava>();

        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;

        private LavaEventConfig _config;

        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("LavaWorldEvent started");
            _config = (LavaEventConfig) config;
            var spawner = new CircleLavaSpawner(_config, _world);
            spawner.SpawnLava(CreateLava);
            yield return WaitFinish(_config);
        }

        private void CreateLava(Vector3 place)
        {
            var lava = _worldObjectFactory.CreateObject<Lava>(_config.LavaPrefabId);
            lava.transform.SetPositionAndRotation(place, Quaternion.identity);
            lava.Init(_config);
            _createdLava.Add(lava);
        }

        protected override void Dispose()
        {
            _createdLava.ForEach(it => { it.Dispose(); });
            _createdLava.Clear();
        }
    }
}
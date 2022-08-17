using System;
using System.Collections;
using System.Collections.Generic;
using Logger.Extension;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Lava
{
    public class LavaWorldEvent : WorldEvent
    {
        private readonly List<Lava> _lava = new List<Lava>();
        
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        
        public override event Action OnFinished;
        
        public override void Start()
        {
            this.Logger().Trace("LavaWorldEvent started");
            
            var config = new LavaEventConfig();
            SpawnLava(config);
            
            GameApplication.Instance.StartCoroutine(WaitFinish(config));
        }
        
        private void SpawnLava(LavaEventConfig config)
        {
            var lava = _worldObjectFactory.CreateObject<Lava>("Lava");
            lava.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            lava.Init(config);
            _lava.Add(lava);
        }
        private IEnumerator WaitFinish(LavaEventConfig config)
        {
            yield return new WaitForSeconds(config.EventDuration);
            DisposeLava();
            this.Logger().Trace("LavaWorldEvent finished");
            OnFinished?.Invoke();
        }

        private void DisposeLava()
        {
            _lava.ForEach(it => {
                it.Dispose();
            });
            _lava.Clear();
        }
    }
}
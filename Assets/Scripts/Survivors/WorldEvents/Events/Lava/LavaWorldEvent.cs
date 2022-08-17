using System;
using System.Collections;
using Logger.Extension;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    public class LavaWorldEvent : WorldEvent
    {
        public override event Action OnFinished;
        
        public override void Start()
        {
            this.Logger().Trace("LavaWorldEvent started");
            GameApplication.Instance.StartCoroutine(WaitFinish());
        }
        private IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(1);
            this.Logger().Trace("LavaWorldEvent finished");
            OnFinished?.Invoke();
        }
    }
}
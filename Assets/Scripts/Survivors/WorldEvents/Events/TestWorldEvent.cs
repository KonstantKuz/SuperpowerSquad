using System;
using System.Collections;
using Logger.Extension;
using Survivors.WorldEvents.Events.Lava;
using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    public class TestWorldEvent : WorldEvent
    {
        private const float TEST_EVENT_DURATION = 5f;
        public override event Action OnFinished;
        
        public override void Start(EventConfig eventConfig)
        {
            this.Logger().Trace("TestWorldEvent started");
            GameApplication.Instance.StartCoroutine(WaitFinish());
        }
        private IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(TEST_EVENT_DURATION);
            this.Logger().Trace("TestWorldEvent finished");
            OnFinished?.Invoke();
        }
    }
}
using System.Collections;
using Logger.Extension;
using Survivors.WorldEvents.Events.Lava;
using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    public class TestWorldEvent : WorldEvent
    {
        private const float TEST_EVENT_DURATION = 5f;

        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("TestWorldEvent started");
            return WaitFinish();
        }
        private IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(TEST_EVENT_DURATION);
            this.Logger().Trace("TestWorldEvent finished");
        }
    }
}
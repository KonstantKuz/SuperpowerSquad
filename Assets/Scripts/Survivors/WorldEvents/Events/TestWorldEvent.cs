using System.Collections;
using Logger.Extension;

namespace Survivors.WorldEvents.Events
{
    public class TestWorldEvent : WorldEvent
    {
        public override IEnumerator Start(EventConfig config)
        {
            this.Logger().Trace("TestWorldEvent started");
            yield return WaitFinish(config);
        }

        protected override void Term()
        {
           
        }
    }
}
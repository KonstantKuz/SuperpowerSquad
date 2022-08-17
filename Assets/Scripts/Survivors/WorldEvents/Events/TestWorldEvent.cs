using System;
using Logger.Extension;

namespace Survivors.WorldEvents.Events
{
    public class TestWorldEvent : WorldEvent
    {
        public override event Action OnFinished;
        
        public override void Start()
        {
            this.Logger().Trace("TestWorldEvent is started");
            OnFinished?.Invoke();
        }
    }
}
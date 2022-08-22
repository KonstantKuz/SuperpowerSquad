using System.Collections;
using Logger.Extension;
using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    public abstract class WorldEvent
    {
        public abstract IEnumerator Start(EventConfig config);
        
        protected IEnumerator WaitFinish(EventConfig config)
        {
            yield return new WaitForSeconds(config.EventDuration);
            Dispose();
            this.Logger().Trace("WorldEvent finished");
        }

        protected abstract void Dispose();


    }
}
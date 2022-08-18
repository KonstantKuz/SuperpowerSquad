using System.Collections;
using Survivors.WorldEvents.Events.Lava;

namespace Survivors.WorldEvents.Events
{
    public abstract class WorldEvent
    {
        public abstract IEnumerator Start(EventConfig config);
        
    }
}
using System.Collections;

namespace Survivors.WorldEvents.Events
{
    public abstract class WorldEvent
    {
        public abstract IEnumerator Start();
        
    }
}
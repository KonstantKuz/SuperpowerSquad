using System;

namespace Survivors.WorldEvents.Events
{
    public abstract class WorldEvent
    { 
        public abstract event Action OnFinished;
        public abstract void Start();
    }
}
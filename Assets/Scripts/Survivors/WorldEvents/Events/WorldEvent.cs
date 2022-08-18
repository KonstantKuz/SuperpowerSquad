using System;
using Survivors.WorldEvents.Events.Lava;

namespace Survivors.WorldEvents.Events
{
    public abstract class WorldEvent
    { 
        public abstract event Action OnFinished;

        public abstract void Start(EventConfig eventConfig);

    }
}
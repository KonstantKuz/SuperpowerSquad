using System;
using System.Collections.Generic;
using Survivors.WorldEvents.Config;
using Survivors.WorldEvents.Events;
using Zenject;

namespace Survivors.WorldEvents.Service
{
    public class WorldEventFactory
    {
        private readonly Dictionary<WorldEventType, Type> _worldEvents = new Dictionary<WorldEventType, Type>() {
                {WorldEventType.Test, typeof(TestWorldEvent)}

        };
        [Inject]
        private DiContainer _diContainer;

        public WorldEvent CreateEvent(WorldEventType eventType)
        {
            if (!_worldEvents.ContainsKey(eventType)) {
                throw new KeyNotFoundException($"IWorldEvent not found for eventType:= {eventType}");
            }
            return (WorldEvent) _diContainer.Instantiate(_worldEvents[eventType]);
        }
    }
}
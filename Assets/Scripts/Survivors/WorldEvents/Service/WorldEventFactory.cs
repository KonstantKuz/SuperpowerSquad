using System;
using System.Collections.Generic;
using Feofun.Util.SerializableDictionary;
using Survivors.WorldEvents.Config;
using Survivors.WorldEvents.Events;
using Survivors.WorldEvents.Events.Avalanche;
using Survivors.WorldEvents.Events.Lava;
using Survivors.WorldEvents.Events.Tornado;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Service
{
    public class WorldEventFactory : MonoBehaviour
    {
        private readonly Dictionary<WorldEventType, Type> _worldEvents = new Dictionary<WorldEventType, Type>() {
                {WorldEventType.Test, typeof(TestWorldEvent)},
                {WorldEventType.Avalanche, typeof(AvalancheEvent)},
                {WorldEventType.Lava, typeof(LavaWorldEvent)},       
                {WorldEventType.Tornado, typeof(TornadoWorldEvent)},
        };
        [Inject]
        private DiContainer _diContainer;

        [SerializeField]
        private SerializableDictionary<WorldEventType, EventConfig> _configs;

        public WorldEvent CreateEvent(WorldEventType eventType)
        {
            if (!_worldEvents.ContainsKey(eventType)) {
                throw new KeyNotFoundException($"IWorldEvent not found for eventType:= {eventType}");
            }
            return (WorldEvent) _diContainer.Instantiate(_worldEvents[eventType]);
        }   
        public EventConfig GetConfig(WorldEventType eventType)
        {
            if (!_configs.ContainsKey(eventType)) {
                throw new KeyNotFoundException($"IWorldEvent not found for eventType:= {eventType}");
            }
            return _configs[eventType];
        }
    }
}
using System.Collections.Generic;

namespace Survivors.WorldEvents.Config
{
    public class LevelEventConfig
    {
        public readonly int Level;
        
        private readonly IReadOnlyList<EventConfig> _events;

        public LevelEventConfig(int level, IReadOnlyList<EventConfig> events)
        {
            Level = level;
            _events = events;
        }
    }
}
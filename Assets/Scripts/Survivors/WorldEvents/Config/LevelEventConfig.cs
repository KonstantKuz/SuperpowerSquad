using System.Collections.Generic;

namespace Survivors.WorldEvents.Config
{
    public class LevelEventConfig
    {
        public readonly string LevelId;
        
        public readonly IReadOnlyList<WorldEventConfig> Events;

        public LevelEventConfig(string levelId, IReadOnlyList<WorldEventConfig> events)
        {
            LevelId = levelId;
            Events = events;
        }
    }
}
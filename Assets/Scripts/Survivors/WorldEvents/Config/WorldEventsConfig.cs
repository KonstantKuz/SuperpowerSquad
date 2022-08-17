using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.WorldEvents.Config
{
    public class WorldEventsConfig : ILoadableConfig
    {
        private IReadOnlyDictionary<string, LevelEventConfig> _levelEvents;
        public void Load(Stream stream)
        {
            _levelEvents = new CsvSerializer().ReadNestedTable<WorldEventConfig>(stream)
                                              .ToDictionary(it => it.Key, it
                                                                    => new LevelEventConfig(it.Key, it.Value));
        }
        private LevelEventConfig GetLevelEventConfig(string levelId)
        {
            if (!_levelEvents.ContainsKey(levelId)) {
                throw new KeyNotFoundException($"LevelEventConfig not found for levelId:= {levelId}");
            }
            return _levelEvents[levelId];
        }
        public IEnumerable<WorldEventConfig> GetEventConfigs(string levelId)
        {
            return GetLevelEventConfig(levelId).Events;
        }
    }
}
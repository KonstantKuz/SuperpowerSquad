using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Loot.Config
{
    public class LootConfig : ILoadableConfig
    {
        private Dictionary<string, IReadOnlyList<DroppingLootConfig>> DroppingLootMap { get; set; }
        
        public void Load(Stream stream)
        {
            DroppingLootMap = new CsvSerializer().ReadNestedTable<DroppingLootConfig>(stream).ToDictionary(it => it.Key, it => it.Value);
        }

        public IReadOnlyList<DroppingLootConfig> FindPossibleLootsFor(string emitterId)
        {
            if (!DroppingLootMap.ContainsKey(emitterId)) {
                return null;
            }

            return DroppingLootMap[emitterId];
        }
    }
}
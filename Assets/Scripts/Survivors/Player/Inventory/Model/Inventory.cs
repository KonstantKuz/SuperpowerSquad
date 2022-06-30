using System.Collections.Generic;
using Newtonsoft.Json;

namespace Survivors.Player.Inventory.Model
{
    public class Inventory
    {
        [JsonProperty]
        private Dictionary<string, int> _metaUpgrades = new Dictionary<string, int>();
       
        private int GetUpgradeCount(string upgradeId) => _metaUpgrades.ContainsKey(upgradeId) ? _metaUpgrades[upgradeId] : 0;
        
        public void AddUpgrade(string upgradeId)
        {
            _metaUpgrades[upgradeId] = GetUpgradeCount(upgradeId) + 1;
        }

        public Dictionary<string, int> MetaUpgrades => _metaUpgrades;
    }
}
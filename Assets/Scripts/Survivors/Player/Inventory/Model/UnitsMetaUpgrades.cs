using System.Collections.Generic;
using Newtonsoft.Json;

namespace Survivors.Player.Inventory.Model
{
    public class UnitsMetaUpgrades
    {
        [JsonProperty]
        private Dictionary<string, int> _upgrades = new Dictionary<string, int>();
       
        public int GetUpgradeLevel(string upgradeId) => _upgrades.ContainsKey(upgradeId) ? _upgrades[upgradeId] : 0;
        
        public void AddUpgrade(string upgradeId)
        {
            _upgrades[upgradeId] = GetUpgradeLevel(upgradeId) + 1;
        }
        public Dictionary<string, int> Upgrades => _upgrades;
    }
}
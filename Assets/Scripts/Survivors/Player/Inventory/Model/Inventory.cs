using System.Collections.Generic;
using Newtonsoft.Json;

namespace Survivors.Player.Inventory.Model
{
    public class Inventory
    {
        public UnitsMetaUpgrades UnitsUpgrades { get; }
        public Inventory()
        {
            UnitsUpgrades = new UnitsMetaUpgrades();
        }
    }

    public class UnitsMetaUpgrades
    {
        [JsonProperty]
        private Dictionary<string, int> _upgrades = new Dictionary<string, int>();
       
        public int GetUpgradeCount(string upgradeId) => _upgrades.ContainsKey(upgradeId) ? _upgrades[upgradeId] : 0;
        
        public void AddUpgrade(string upgradeId)
        {
            _upgrades[upgradeId] = GetUpgradeCount(upgradeId) + 1;
        }
        public Dictionary<string, int> Upgrades => _upgrades;
    }
}
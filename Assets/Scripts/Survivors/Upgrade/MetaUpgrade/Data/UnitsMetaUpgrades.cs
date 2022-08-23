using System.Collections.Generic;
using Logger.Extension;
using Newtonsoft.Json;
using SuperMaxim.Core.Extensions;

namespace Survivors.Upgrade.MetaUpgrade.Data
{
    public class UnitsMetaUpgrades
    {
        [JsonProperty]
        private Dictionary<string, UpgradeState> _upgrades = new Dictionary<string, UpgradeState>();      
        
        public int GetUpgradeLevel(string upgradeId) => _upgrades.ContainsKey(upgradeId) ? _upgrades[upgradeId].Level : 0;
        
        public void AddUpgrade(string upgradeId)
        {
            if (!_upgrades.ContainsKey(upgradeId)) {
                _upgrades[upgradeId] = new UpgradeState();
            }
            _upgrades[upgradeId].Level = GetUpgradeLevel(upgradeId) + 1;
        }   
        public void SetBoughtStateForCurrency(string upgradeId)
        {
            if (!_upgrades.ContainsKey(upgradeId)) {
                this.Logger().Error($"Upgrade not found, upgrade needs to be added, upgradeId:= {upgradeId}");
                return;
            }
            _upgrades[upgradeId].IsPurchasedWithCurrency = true;
        } 
        public bool IsPurchasedWithCurrency(string upgradeId)
        {
            return _upgrades.ContainsKey(upgradeId) && _upgrades[upgradeId].IsPurchasedWithCurrency;
        }
        public void ResetBoughtStateForCurrency()
        {
            _upgrades.Values.ForEach(it => {
                it.IsPurchasedWithCurrency = false;
            });
        }
    }
}
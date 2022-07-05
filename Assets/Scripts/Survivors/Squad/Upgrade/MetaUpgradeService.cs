using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Logger.Extension;
using ModestTree;
using Survivors.App.Config;
using Survivors.Config;
using Survivors.Location;
using Survivors.Modifiers.Config;
using Survivors.Player.Inventory.Model;
using Survivors.Player.Inventory.Service;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    [PublicAPI]
    public class MetaUpgradeService : IWorldScope
    {
        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private World _world;
        [Inject(Id = Configs.META_UPGRADES)]
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject]
        private readonly ModifierFactory _modifierFactory;
        [Inject]
        private ConstantsConfig _constantsConfig;

        public UnitsMetaUpgrades MetaUpgrades => _inventoryService.Inventory.UnitsUpgrades;

        public void OnWorldSetup()
        {
        }

        public bool IsMaxLevel(string upgradeId)
        {
            return MetaUpgrades.GetUpgradeLevel(upgradeId) >= _constantsConfig.MaxMetaUpgradeLevel;
        }  
        public int GetLevel(string upgradeId)
        {
            return MetaUpgrades.GetUpgradeLevel(upgradeId);
        }   
        public int GetNextLevel(string upgradeId)
        {
            var level = MetaUpgrades.GetUpgradeLevel(upgradeId);
            if (IsMaxLevel(upgradeId)) {
                return level;
            }
            return level + 1;
        }
        public void Upgrade(string upgradeId)
        {
            if (IsMaxLevel(upgradeId)) {
                this.Logger().Error($"Meta upgrade error, upgrade: {upgradeId} is max level, level:{MetaUpgrades.GetUpgradeLevel(upgradeId)}");
                return;
            }
            _inventoryService.AddUpgrade(upgradeId);
            ApplyUpgrade(upgradeId);
        }
        private void ApplyUpgrade(string upgradeId)
        {
            var modificatorConfig = _modifierConfigs.Find(upgradeId);
            if (modificatorConfig == null) return;
            var modificator = _modifierFactory.Create(modificatorConfig.ModifierConfig);
            Assert.IsNotNull(_world.Squad, "Squad is null, should call this method only inside game session");
            _world.Squad.AddModifier(modificator, modificatorConfig.Target);
        }
        public void OnWorldCleanUp()
        {
            
        }
    }
}
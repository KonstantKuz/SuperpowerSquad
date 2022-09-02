using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Logger.Extension;
using ModestTree;
using Survivors.App.Config;
using Survivors.Config;
using Survivors.Location;
using Survivors.Modifiers.Config;
using Survivors.Upgrade.MetaUpgrade.Data;
using Zenject;

namespace Survivors.Upgrade.MetaUpgrade
{
    [PublicAPI]
    public class MetaUpgradeService : IWorldScope
    {
        [Inject]
        private MetaUpgradeRepository _repository;
        [Inject]
        private World _world;
        [Inject(Id = Configs.META_UPGRADES)]
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject]
        private readonly ModifierFactory _modifierFactory;
        [Inject]
        private ConstantsConfig _constantsConfig;
        [Inject] 
        private Analytics.Analytics _analytics;

        public UnitsMetaUpgrades MetaUpgrades
        {
            get
            {
                if (!_repository.Exists()) {
                    _repository.Set(new UnitsMetaUpgrades());
                }
                return _repository.Get();
            }
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
        public void Upgrade(string upgradeId, bool boughtWithCurrency = false)
        {
            if (IsMaxLevel(upgradeId)) {
                this.Logger().Error($"Meta upgrade error, upgrade: {upgradeId} is max level, level:{MetaUpgrades.GetUpgradeLevel(upgradeId)}");
                return;
            }
            AddUpgrade(upgradeId);
            ApplyUpgrade(upgradeId);
            if (boughtWithCurrency) {
                SetBoughtStateForCurrency(upgradeId);
            }
            _analytics.ReportMetaUpgradeLevelUp(upgradeId);
        }
 
        public bool IsPurchasedWithCurrency(string upgradeId) => MetaUpgrades.IsPurchasedWithCurrency(upgradeId);
        public void ResetUpgrades()
        {
            _repository.Set(new UnitsMetaUpgrades());
        }
        private void ApplyUpgrade(string upgradeId)
        {
            var modificatorConfig = _modifierConfigs.Find(upgradeId);
            if (modificatorConfig == null) return;
            var modificator = _modifierFactory.Create(modificatorConfig.ModifierConfig);
            Assert.IsNotNull(_world.Squad, "Squad is null, should call this method only inside game session");
            _world.Squad.AddModifier(modificator, modificatorConfig.Target);
        }
        private void SetBoughtStateForCurrency(string upgradeId)
        {
            var upgrades = MetaUpgrades;
            upgrades.SetBoughtStateForCurrency(upgradeId);
            Set(upgrades);
        }
        private void AddUpgrade(string upgradeId)
        {
            var upgrades = MetaUpgrades;
            upgrades.AddUpgrade(upgradeId);
            Set(upgrades);
        }
        private void Set(UnitsMetaUpgrades model)
        {
            _repository.Set(model);
        }
        
        public void OnWorldSetup()
        {
            
        }
        public void OnWorldCleanUp()
        {
            ResetBoughtStateForCurrency();
        }

        private void ResetBoughtStateForCurrency()
        {
            var upgrades = MetaUpgrades;
            upgrades.ResetBoughtStateForCurrency();
            Set(upgrades);
        }
    }
}
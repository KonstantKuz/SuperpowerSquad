using System;
using System.Linq;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using LegionMaster.Extension;
using Survivors.Location;
using Survivors.Units.Modifiers;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    [PublicAPI]
    public class UpgradeService
    {
        private SquadState _squadState;
        
        [Inject] private UpgradesConfig _config;

        [Inject] private World _world;

        [Inject] private UnitFactory _unitFactory;

        [Inject] private ModifierFactory _modifierFactory;
        
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;

        public void Init()
        {
            _squadState = new SquadState();
        }

        public void AddRandomUpgrade()
        {
            Upgrade(_config.Keys().ToList().Random());
        }
        
        public void Upgrade(string upgradeId)
        {
            var level = _squadState.GetLevel(upgradeId);
            if (level >= _config.GetMaxLevel(upgradeId)) return;
            _squadState.IncreaseLevel(upgradeId);
            var upgradeConfig = _config.GetUpgradeConfig(upgradeId, _squadState.GetLevel(upgradeId));
            ApplyUpgrade(upgradeConfig);
        }

        private void ApplyUpgrade(UpgradeConfig upgradeConfig)
        {
            switch (upgradeConfig.Type)
            {
                case UpgradeType.Unit:
                    AddUnit(upgradeConfig);
                    break;
                case UpgradeType.Modifier:
                    AddModifier(upgradeConfig);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddModifier(UpgradeConfig upgradeConfig)
        {
            var modifierConfig = _modifierConfigs.Get(upgradeConfig.UpgradeId).ModifierConfig;
            _world.Squad.AddModifier(_modifierFactory.Create(modifierConfig));
        }

        private void AddUnit(UpgradeConfig upgradeConfig)
        {
            _unitFactory.CreatePlayerUnit(upgradeConfig.UpgradeId);
        }
    }
}
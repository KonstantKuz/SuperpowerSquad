using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using LegionMaster.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Squad.Upgrade.Config;
using Survivors.Units;
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
            _squadState = InitSquadState();
        }

        private static SquadState InitSquadState()
        {
            var squadState = new SquadState();
            squadState.IncreaseLevel(UnitFactory.SIMPLE_PLAYER_ID);
            return squadState;
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
            ApplyUpgrade(upgradeConfig, upgradeId);
        }

        private void ApplyUpgrade(UpgradeConfig upgradeConfig, string upgradeId)
        {
            switch (upgradeConfig.Type)
            {
                case UpgradeType.Unit:
                    AddUnit(upgradeConfig);
                    break;
                case UpgradeType.Modifier:
                    AddModifier(upgradeConfig, upgradeId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddModifier(UpgradeConfig upgradeConfig, string upgradeId)
        {
            var modifier = CreateModifier(upgradeConfig);
            var targetUnits = GetTargetUnits(upgradeId);            
            targetUnits.ForEach(it => it.AddModifier(modifier));
        }

        private IModifier CreateModifier(UpgradeConfig upgradeConfig)
        {
            var modifierConfig = _modifierConfigs.Get(upgradeConfig.ImprovementId).ModifierConfig;
            var modifier = _modifierFactory.Create(modifierConfig);
            return modifier;
        }

        private IEnumerable<Unit> GetTargetUnits(string upgradeId)
        {
            var allSquadUnits = _world.Squad.Units;
            return !_config.IsUnitUpgrade(upgradeId) ? 
                allSquadUnits : 
                allSquadUnits.Where(it => it.Model.Id == _config.GetUnitName(upgradeId));
        }

        private void AddUnit(UpgradeConfig upgradeConfig)
        {
            var unit = _unitFactory.CreatePlayerUnit(upgradeConfig.ImprovementId);
            AddExistingModifiers(unit);
        }

        private void AddExistingModifiers(Unit newUnit)
        {
            foreach (var upgrade in _squadState.Upgrades)
            {
                if (IsDifferentUnitUpgrade(newUnit, upgrade.Key)) continue;
                for (int level = 1; level <= upgrade.Value; level++)
                {
                    var upgradeConfig = _config.GetUpgradeConfig(upgrade.Key, level);
                    if (upgradeConfig.Type == UpgradeType.Unit) continue;
                    var modifier = CreateModifier(upgradeConfig);
                    newUnit.AddModifier(modifier);
                }
            }
        }

        private bool IsDifferentUnitUpgrade(Unit newUnit, string upgradeId)
        {
            return _config.IsUnitUpgrade(upgradeId) && _config.GetUnitName(upgradeId) == newUnit.Model.Id;
        }
    }
}
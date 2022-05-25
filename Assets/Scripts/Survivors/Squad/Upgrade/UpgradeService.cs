using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using LegionMaster.Extension;
using Survivors.Location;
using Survivors.Modifiers;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade.Config;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    [PublicAPI]
    public class UpgradeService
    {
        private SquadUpgradeState _squadUpgradeState;
        
        [Inject] private UpgradesConfig _config;

        [Inject] private World _world;

        [Inject] private UnitFactory _unitFactory;

        [Inject] private ModifierFactory _modifierFactory;
        
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;

        public void Init()
        {
            _squadUpgradeState = InitSquadState();
        }

        private static SquadUpgradeState InitSquadState()
        {
            var squadState = new SquadUpgradeState();
            squadState.IncreaseLevel(UnitFactory.SIMPLE_PLAYER_ID);
            return squadState;
        }

        public void AddRandomUpgrade()
        {
            Upgrade(_config.GetUpgradeBranchIds().ToList().Random());
        }
        
        public void Upgrade(string upgradeBranchId)
        {
            var level = _squadUpgradeState.GetLevel(upgradeBranchId);
            if (level >= _config.GetMaxLevel(upgradeBranchId)) return;
            _squadUpgradeState.IncreaseLevel(upgradeBranchId);
            ApplyUpgrade(upgradeBranchId, _squadUpgradeState.GetLevel(upgradeBranchId));
        }

        private void ApplyUpgrade(string upgradeBranchId, int level)
        {
            var upgradeBranch = _config.GetUpgradeBranch(upgradeBranchId);
            var upgradeConfig = upgradeBranch.GetLevel(level);
            
            switch (upgradeConfig.Type)
            {
                case UpgradeType.Unit:
                    AddUnit(upgradeBranch.BranchUnitName);
                    break;
                case UpgradeType.Modifier:
                    AddModifier(upgradeConfig, upgradeBranch.BranchUnitName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddModifier(UpgradeLevelConfig upgradeLevelConfig, [CanBeNull]string unitName)
        {
            var modifierConfig = _modifierConfigs.Get(upgradeLevelConfig.ModifierId);
            var modifier = _modifierFactory.Create(modifierConfig.ModifierConfig);
            _world.Squad.AddModifier(modifier, modifierConfig.Target, unitName);
        }

        private void AddUnit(string unitId)
        {
            var unit = _unitFactory.CreatePlayerUnit(unitId);
            AddExistingModifiers(unit);
        }

        private void AddExistingModifiers(Unit newUnit)
        {
            var existingUpgrades = new List<Tuple<string, int>>();
            existingUpgrades.AddRange(GetUnitUpgrades(newUnit.Model.Id));
            existingUpgrades.AddRange(GetAbilitiesUpgrades());
            
            foreach (var (upgradeId, level) in existingUpgrades)
            {
                var upgradeConfig = _config.GetUpgradeConfig(upgradeId, level);
                if (upgradeConfig.Type == UpgradeType.Unit) continue;
                
                var modifierConfig = _modifierConfigs.Get(upgradeConfig.ModifierId);
                if (modifierConfig.Target == ModifierTarget.Squad) continue;
                
                var modifier = _modifierFactory.Create(modifierConfig.ModifierConfig);
                newUnit.AddModifier(modifier);
            }
        }

        private IEnumerable<Tuple<string, int>> GetAbilitiesUpgrades()
        {
            foreach (var upgrade in _squadUpgradeState.Upgrades)
            {
                var upgradeBranch = _config.GetUpgradeBranch(upgrade.Key);
                if (upgradeBranch.IsUnitBranch) continue;
                for (int level = 1; level <= upgrade.Value; level++)
                {
                    yield return new Tuple<string, int>(upgrade.Key, level);
                }
            }
        }

        private IEnumerable<Tuple<string, int>> GetUnitUpgrades(string unitId)
        {
            var unitLevel = _squadUpgradeState.GetLevel(unitId);
            for (int level = 1; level < unitLevel; level++)
            {
                yield return new Tuple<string, int>(unitId, level);
            }
        }
    }
}
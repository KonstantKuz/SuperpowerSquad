using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Extension;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Survivors.Location;
using Survivors.Modifiers;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade.Config;
using Survivors.Squad.UpgradeSelection;
using Survivors.Units;
using Survivors.Units.Service;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    [PublicAPI]
    public class UpgradeService : IWorldScope
    {
        [Inject] private UpgradesConfig _config;
        [Inject] private World _world;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private ModifierFactory _modifierFactory;
        [Inject] private SquadUpgradeRepository _repository;
        
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        public SquadUpgradeState SquadUpgradeState => _repository.Require();
      
        public void OnWorldSetup()
        {
            _repository.Set(SquadUpgradeState.Create());
        }

        public void AddRandomUpgrade()
        {
            Upgrade(_config.GetUpgradeBranchIds().ToList().Random());
        } 
        public void ApplyAllUpgrades()
        {
            foreach (string upgradeBranchId in _config.GetUpgradeBranchIds()) {
                var upgradeBranchConfig = _config.GetUpgradeBranch(upgradeBranchId);
                for (int i = 0; i < upgradeBranchConfig.MaxLevel; i++) {
                    Upgrade(upgradeBranchId);
                }
            }
        }
        
        public void Upgrade(string upgradeBranchId)
        {
            if (SquadUpgradeState.IsMaxLevel(upgradeBranchId, _config)) return;
            var state = SquadUpgradeState;
            state.IncreaseLevel(upgradeBranchId);
            SaveState(state);
            ApplyUpgrade(upgradeBranchId, SquadUpgradeState.GetLevel(upgradeBranchId));
            Debug.Log($"Upgrade:={upgradeBranchId} applied, level:= {state.GetLevel(upgradeBranchId)}");
        }

        private void ApplyUpgrade(string upgradeBranchId, int level)
        {
            var upgradeBranch = _config.GetUpgradeBranch(upgradeBranchId);
            var upgradeConfig = upgradeBranch.GetLevel(level);
            
            switch (upgradeConfig.Type)
            {
                case UpgradeType.Unit:
                    Assert.IsFalse(upgradeConfig.IsTargetAllUnits);
                    AddUnit(upgradeConfig.TargetId);
                    break;
                case UpgradeType.Modifier: {
                    AddModifier(upgradeConfig);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddModifier(UpgradeLevelConfig upgradeLevelConfig)
        {
            var modifierConfig = _modifierConfigs.Get(upgradeLevelConfig.ModifierId);
            var modifier = _modifierFactory.Create(modifierConfig.ModifierConfig);
            _world.Squad.AddModifier(modifier, modifierConfig.Target, upgradeLevelConfig.TargetId);
        }

        private void AddUnit(string unitId)
        {
            var unit = _unitFactory.CreatePlayerUnit(unitId);
            AddExistingModifiers(unit);
        }

        private void AddExistingModifiers(Unit newUnit)
        {
            var unitId = newUnit.Model.Id;
            var existingUpgrades = new List<Tuple<string, int>>();
            existingUpgrades.AddRange(GetUnitUpgrades(unitId));
            existingUpgrades.AddRange(GetAbilitiesUpgrades(unitId));
            
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

        private IEnumerable<Tuple<string, int>> GetAbilitiesUpgrades(string unitId)
        {
            foreach (var upgrade in SquadUpgradeState.Upgrades)
            {
                var upgradeBranch = _config.GetUpgradeBranch(upgrade.Key);
                if (upgradeBranch.BranchType == UpgradeBranchType.Unit) continue;

                for (int level = 1; level <= upgrade.Value; level++) {
                    var levelConfig = upgradeBranch.GetLevel(level);
                    if (levelConfig.IsValidTarget(unitId)) {
                        yield return new Tuple<string, int>(upgrade.Key, level);
                    }
                }
            }
        }

        private IEnumerable<Tuple<string, int>> GetUnitUpgrades(string unitId)
        {
            var unitLevel = SquadUpgradeState.GetLevel(unitId);
            for (int level = 1; level < unitLevel; level++)
            {

                yield return new Tuple<string, int>(unitId, level);
            }
        }
        private void SaveState(SquadUpgradeState state)
        {
            _repository.Set(state);
        }
        private void ResetProgress()
        {
            _repository.Delete();
        }
        public void OnWorldCleanUp()
        {
            ResetProgress();
        }


    }
}
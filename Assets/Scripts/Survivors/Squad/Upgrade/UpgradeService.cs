using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using LegionMaster.Extension;
using SuperMaxim.Core.Extensions;
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
                    AddUnit(upgradeId);
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
            var modifierConfig = _modifierConfigs.Get(upgradeConfig.ModifierId).ModifierConfig;
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
                
                var modifier = CreateModifier(upgradeConfig);
                newUnit.AddModifier(modifier);
            }
        }

        private IEnumerable<Tuple<string, int>> GetAbilitiesUpgrades()
        {
            foreach (var upgrade in _squadState.Upgrades)
            {
                if (_config.IsUnitUpgrade(upgrade.Key)) continue;
                for (int level = 1; level <= upgrade.Value; level++)
                {
                    yield return new Tuple<string, int>(upgrade.Key, level);
                }
            }
        }

        private IEnumerable<Tuple<string, int>> GetUnitUpgrades(string unitId)
        {
            var unitLevel = _squadState.GetLevel(unitId);
            for (int level = 1; level < unitLevel; level++)
            {
                yield return new Tuple<string, int>(unitId, level);
            }
        }
    }
}
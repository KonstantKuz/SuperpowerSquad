﻿using System;
using System.Linq;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using LegionMaster.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Units.Modifiers;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    //TODO: надо добавлять модификаторы новым юнитам
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
            var modifierConfig = _modifierConfigs.Get(upgradeConfig.UpgradeId).ModifierConfig;
            var allSquadUnits = _world.Squad.Units;
            var units = modifierConfig.Target switch
            {
                ModifierTarget.Friends => allSquadUnits,
                ModifierTarget.Self => allSquadUnits.Where(it => it.Model.Id == upgradeId), //TODO: есть неявная завязка на то, что Target.Self стоит у апгрейдов юнитов...
                _ => throw new ArgumentOutOfRangeException()
            };
            var modifier = _modifierFactory.Create(modifierConfig);
            units.ForEach(it => it.AddModifier(modifier));
        }

        private void AddUnit(UpgradeConfig upgradeConfig)
        {
            _unitFactory.CreatePlayerUnit(upgradeConfig.UpgradeId);
        }
    }
}
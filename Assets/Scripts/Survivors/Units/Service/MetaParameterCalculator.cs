using Feofun.Config;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers.Config;
using Survivors.Player.Inventory.Model;

namespace Survivors.Units.Service
{
    public class MetaParameterCalculator
    {
        private readonly UnitsMetaUpgrades _unitsUpgrades;
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        private readonly ModifierFactory _modifierFactory;

        public MetaParameterCalculator(UnitsMetaUpgrades unitsUpgrades,
                                       StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                       ModifierFactory modifierFactory)
        {
            _unitsUpgrades = unitsUpgrades;
            _modifierConfigs = modifierConfigs;
            _modifierFactory = modifierFactory;
        }
        public FloatModifiableParameter CalculateParam(FloatModifiableParameter parameter, IModifiableParameterOwner owner)
        {
            parameter.Reset();
            var upgradeCount = _unitsUpgrades.GetUpgradeCount(parameter.Name);
            if (upgradeCount <= 0) return parameter;
            var modificatorConfig = _modifierConfigs.Find(parameter.Name);
            if (modificatorConfig == null) return parameter;
            var modificator = _modifierFactory.Create(modificatorConfig.ModifierConfig);
            for (int i = 0; i < upgradeCount; i++) {
                modificator.Apply(owner);
            }
            return parameter;
        }
    }
}
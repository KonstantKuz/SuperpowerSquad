using System;
using Feofun.Modifiers.Config;

namespace Feofun.Modifiers
{
    public static class ModifierFactory
    {
        public static IModifier Create(ModifierConfig modifierCfg)
        {
            return modifierCfg.Modifier switch
            {
                ModifierType.AddValue => new AddValueModifier(modifierCfg.ParameterName, modifierCfg.Value),
                ModifierType.AddPercent => new AddPercentModifier(modifierCfg.ParameterName, modifierCfg.Value),
                _ => throw new ArgumentOutOfRangeException($"Unsupported modifier type {modifierCfg.Modifier}")
            };
        }
    }
}
using System;
using System.Collections.Generic;
using Feofun.Modifiers.Config;

namespace Feofun.Modifiers
{
    public class ModifierFactory
    {
        private readonly Dictionary<string, Func<ModifierConfig, IModifier>> _factoryMethods = 
            new Dictionary<string, Func<ModifierConfig, IModifier>>();

        public void Register(string name, Func<ModifierConfig, IModifier> factoryMethod)
        {
            _factoryMethods[name] = factoryMethod;
        }
        public IModifier Create(ModifierConfig modifierCfg)
        {
            if (!_factoryMethods.ContainsKey(modifierCfg.Modifier))
            {
                throw new Exception($"no modifier factory registered for modifier {modifierCfg.Modifier}");
            }

            return _factoryMethods[modifierCfg.Modifier].Invoke(modifierCfg);
        }
    }
}
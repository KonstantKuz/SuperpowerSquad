using Feofun.Modifiers;
using Feofun.Modifiers.Modifiers;
using Zenject;

namespace Survivors.Modifiers
{
    public static class ModifiersInstaller
    {
        public static void Install(DiContainer container)
        {
            var modifierFactory = new ModifierFactory();
            modifierFactory.Register("AddValue", cfg => new AddValueModifier(cfg.ParameterName, cfg.Value));
            modifierFactory.Register("AddPercent", cfg => new AddPercentModifier(cfg.ParameterName, cfg.Value));
            container.Bind<ModifierFactory>().FromInstance(modifierFactory).AsSingle();            
        }
    }
}
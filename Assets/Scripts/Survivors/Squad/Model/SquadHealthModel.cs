using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Service;
using UniRx;

namespace Survivors.Squad.Model
{
    public class SquadHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public SquadHealthModel(IModifiableParameterOwner parameterOwner, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            StartingMaxHealth = startingHealth;
            
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, StartingMaxHealth, parameterOwner);  
            parameterCalculator.InitParam(_maxHealth, parameterOwner);
        }
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}
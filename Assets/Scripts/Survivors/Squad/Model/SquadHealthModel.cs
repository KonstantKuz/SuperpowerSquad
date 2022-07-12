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
        private readonly MetaParameterCalculator _parameterCalculator;    
        private readonly IModifiableParameterOwner _parameterOwner;

        public SquadHealthModel(IModifiableParameterOwner parameterOwner, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            StartingMaxHealth = startingHealth;
            _parameterCalculator = parameterCalculator;
            _parameterOwner = parameterOwner;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, 0, parameterOwner);  
            parameterCalculator.InitParam(_maxHealth, parameterOwner);
        }

        public void Reset()
        {
            _maxHealth.Reset();
            _parameterCalculator.InitParam(_maxHealth, _parameterOwner);
        }

        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}
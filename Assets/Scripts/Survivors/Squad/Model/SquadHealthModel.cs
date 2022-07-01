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
        private readonly MetaParameterCalculator _parameterCalculator;   
        private readonly IModifiableParameterOwner _parameterOwner;
        
        private readonly FloatModifiableParameter _maxHealth;
        private readonly FloatModifiableParameter _startingMaxHealth;

        public SquadHealthModel(IModifiableParameterOwner parameterOwner, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            _parameterOwner = parameterOwner;
            _parameterCalculator = parameterCalculator;
            _startingMaxHealth = new FloatModifiableParameter(Parameters.STARTING_HEALTH, startingHealth, parameterOwner);
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, StartingMaxHealth, parameterOwner);  
        }
        public float StartingMaxHealth => _parameterCalculator.CalculateParam(_startingMaxHealth, _parameterOwner).Value;
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}
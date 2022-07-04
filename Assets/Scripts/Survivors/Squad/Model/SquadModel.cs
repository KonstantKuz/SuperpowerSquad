using Feofun.Modifiers;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Squad.Config;
using Survivors.Units.Model;
using Survivors.Units.Service;
using UniRx;

namespace Survivors.Squad.Model
{
    public class SquadModel : ModifiableParameterOwner
    {
        private readonly FloatModifiableParameter _speed;
        private readonly FloatModifiableParameter _collectRadius;
        private readonly FloatModifiableParameter _startingUnitCount;

        public SquadModel(SquadConfig config, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            _speed = new FloatModifiableParameter(Parameters.SPEED, config.Speed, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);
            
            _startingUnitCount = new FloatModifiableParameter(Parameters.STARTING_UNIT_COUNT, 1, this);
            parameterCalculator.CalculateParam(_startingUnitCount, this);
            
            HealthModel = new SquadHealthModel(this, startingHealth, parameterCalculator);
        }

        public void AddUnit(IUnitModel unitModel)
        {
            var addHealthModifier = new AddValueModifier(Parameters.HEALTH, unitModel.HealthModel.MaxHealth.Value);
            AddModifier(addHealthModifier);
        }

        public IHealthModel HealthModel { get; }
        public int StartingUnitCount => (int) _startingUnitCount.Value;
        public IReadOnlyReactiveProperty<float> Speed => _speed.ReactiveValue;
        public IReadOnlyReactiveProperty<float> CollectRadius => _collectRadius.ReactiveValue;
    }
}
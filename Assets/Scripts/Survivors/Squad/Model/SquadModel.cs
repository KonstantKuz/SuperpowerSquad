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
        private readonly MetaParameterCalculator _parameterCalculator;

        private readonly FloatModifiableParameter _speed;
        private readonly FloatModifiableParameter _collectRadius;
        private readonly FloatModifiableParameter _startingUnitCount;

        public SquadModel(SquadConfig config, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            _parameterCalculator = parameterCalculator;
            _speed = new FloatModifiableParameter(Parameters.SPEED, config.Speed, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);
            _startingUnitCount = new FloatModifiableParameter(Parameters.STARTING_UNIT_COUNT, 1, this);

            HealthModel = new SquadHealthModel(this, startingHealth * StartingUnitCount, parameterCalculator);
        }

        public void AddUnit(IUnitModel unitModel)
        {
            var addHealthModifier = new AddValueModifier(Parameters.HEALTH, unitModel.HealthModel.MaxHealth.Value);
            AddModifier(addHealthModifier);
        }

        public IHealthModel HealthModel { get; }
        public int StartingUnitCount => (int) _parameterCalculator.CalculateParam(_startingUnitCount, this).Value;
        public IReadOnlyReactiveProperty<float> Speed => _speed.ReactiveValue;
        public IReadOnlyReactiveProperty<float> CollectRadius => _collectRadius.ReactiveValue;
    }
}
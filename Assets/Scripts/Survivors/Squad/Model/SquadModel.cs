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
        private readonly FloatModifiableParameter _attackDistance;
        private readonly FloatModifiableParameter _collectRadius;
        private readonly FloatModifiableParameter _startingUnitModifiableCount;
        private readonly IReadOnlyReactiveProperty<int> _startingUnitCount;

        public SquadModel(SquadConfig config, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            _attackDistance = new FloatModifiableParameter(Parameters.ATTACK_DISTANCE, config.AttackDistance, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);

            _startingUnitModifiableCount = new FloatModifiableParameter(Parameters.STARTING_UNIT_COUNT, 1, this);
            parameterCalculator.InitParam(_startingUnitModifiableCount, this);
            _startingUnitCount = _startingUnitModifiableCount.ReactiveValue.Select(it => (int) it).ToReactiveProperty();

            HealthModel = new SquadHealthModel(this, startingHealth, parameterCalculator);
        }

        public void AddUnit(IUnitModel unitModel)
        {
            var addHealthModifier = new AddValueModifier(Parameters.HEALTH, unitModel.HealthModel.MaxHealth.Value);
            AddModifier(addHealthModifier);
        }

        public void ResetHealth()
        {
            var squadHealthModel = (SquadHealthModel) HealthModel;
            squadHealthModel.Reset();
        }

        public IHealthModel HealthModel { get; }
        public IReadOnlyReactiveProperty<float> AttackDistance => _attackDistance.ReactiveValue;
        public IReadOnlyReactiveProperty<int> StartingUnitCount => _startingUnitCount;
        public IReadOnlyReactiveProperty<float> CollectRadius => _collectRadius.ReactiveValue;
    }
}
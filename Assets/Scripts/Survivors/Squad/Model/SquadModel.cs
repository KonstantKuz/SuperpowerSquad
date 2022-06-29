using Feofun.Modifiers;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Squad.Config;
using Survivors.Units.Model;
using UniRx;

namespace Survivors.Squad.Model
{
    public class SquadModel : ModifiableParameterOwner
    {
        private readonly FloatModifiableParameter _speed;
        private readonly FloatModifiableParameter _collectRadius;

        public SquadModel(SquadConfig config, float startingMaxHealth)
        {
            _speed = new FloatModifiableParameter(Parameters.SPEED, config.Speed, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);
            HealthModel = new SquadHealthModel(this, startingMaxHealth);
        }

        public void AddUnit(IUnitModel unitModel)
        {
            var addHealthModifier = new AddValueModifier(Parameters.HEALTH, unitModel.HealthModel.MaxHealth.Value);
            AddModifier(addHealthModifier);
        }
        public IHealthModel HealthModel { get; }
        public IReadOnlyReactiveProperty<float> Speed => _speed.ReactiveValue;
        public IReadOnlyReactiveProperty<float> CollectRadius => _collectRadius.ReactiveValue;
    }
}
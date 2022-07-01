using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using UniRx;

namespace Survivors.Units.Player.Model.Session
{
    public class PlayerHealthSessionModel : IPlayerHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public PlayerHealthSessionModel(IPlayerHealthModel model, IModifiableParameterOwner parameterOwner)
        {
            StartingMaxHealth = model.StartingMaxHealth;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, model.MaxHealth.Value, parameterOwner);
        }

        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}
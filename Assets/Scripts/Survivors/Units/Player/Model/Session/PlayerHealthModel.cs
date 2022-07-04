using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Model;
using UniRx;

namespace Survivors.Units.Player.Model.Session
{
    public class PlayerHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public PlayerHealthModel(float maxHealth, IModifiableParameterOwner parameterOwner)
        {
            StartingMaxHealth = maxHealth;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, maxHealth, parameterOwner);
        }
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}
using Feofun.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Modifiers;

namespace Survivors.Units.Player.Model
{
    public class PlayerHealthModel: IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public PlayerHealthModel(float maxHealth, IModifiableParameterOwner parameterOwner)
        {
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, maxHealth, parameterOwner);
        }

        public float MaxHealth => _maxHealth.Value;
    }
}
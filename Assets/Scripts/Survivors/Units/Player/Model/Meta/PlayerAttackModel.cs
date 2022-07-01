using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using UniRx;

namespace Survivors.Units.Player.Model.Meta
{
    public class PlayerAttackModel : IPlayerAttackModel
    {
        private readonly PlayerAttackConfig _config;
        private readonly FloatModifiableParameter _attackDamage;
        private readonly MetaParameterCalculator _parameterCalculator;   
        private readonly IModifiableParameterOwner _parameterOwner;
        public PlayerAttackModel(PlayerAttackConfig config, IModifiableParameterOwner parameterOwner, MetaParameterCalculator parameterCalculator)
        {
            _config = config;
            _parameterOwner = parameterOwner;
            _parameterCalculator = parameterCalculator;
            AttackInterval = new ReactiveProperty<float>(_config.AttackInterval);
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, config.AttackDamage, parameterOwner);
        }

        public float TargetSearchRadius => AttackDistance;
        public float AttackDistance => _config.AttackDistance;

        public float DamageRadius => _config.DamageRadius;
        public float AttackDamage => _parameterCalculator.CalculateParam(_attackDamage, _parameterOwner).Value;
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }
        public float ProjectileSpeed => _config.ProjectileSpeed;
    }
}
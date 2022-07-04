using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;

namespace Survivors.Units.Player.Model
{
    public class PlayerAttackModel : IAttackModel
    {
        private readonly PlayerAttackConfig _config;
        
        private readonly FloatModifiableParameter _attackDamage;
        private readonly FloatModifiableParameter _attackInterval;
        private readonly FloatModifiableParameter _projectileSpeed;
        private readonly FloatModifiableParameter _damageRadius;
        private readonly FloatModifiableParameter _attackDistance;

        public PlayerAttackModel(PlayerAttackConfig config, IModifiableParameterOwner parameterOwner, MetaParameterCalculator parameterCalculator)
        {
            _config = config;
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, _config.AttackDamage, parameterOwner); 
            parameterCalculator.CalculateParam(_attackDamage, parameterOwner);
            
            _attackInterval = new FloatModifiableParameter(Parameters.ATTACK_INTERVAL, _config.AttackInterval, parameterOwner);
            _attackDistance = new FloatModifiableParameter(Parameters.ATTACK_DISTANCE, _config.AttackDistance, parameterOwner);
            
            _projectileSpeed = new FloatModifiableParameter(Parameters.PROJECTILE_SPEED, _config.ProjectileSpeed, parameterOwner);
            _damageRadius = new FloatModifiableParameter(Parameters.DAMAGE_RADIUS, _config.DamageRadius, parameterOwner);
            
            var shotCount = new FloatModifiableParameter(Parameters.SHOT_COUNT, 1, parameterOwner);
            ShotCount = shotCount.ReactiveValue.Select(it => (int) it).ToReactiveProperty();
        }

        public float TargetSearchRadius => AttackDistance;
        public float AttackDistance => _attackDistance.Value;

        public float DamageRadius => _damageRadius.Value;

        public float AttackDamage => _attackDamage.Value;
        
        public IReadOnlyReactiveProperty<float> AttackInterval => _attackInterval.ReactiveValue;

        public float ProjectileSpeed => _projectileSpeed.Value;
        
        public IReadOnlyReactiveProperty<int> ShotCount { get; }

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams {
                    Speed = ProjectileSpeed,        
                    DamageRadius = DamageRadius,
                    AttackDistance = AttackDistance,
                    Count = ShotCount.Value
            };
        }
        public PlayerProjectileParams CreatePlayerProjectileParams() => new PlayerProjectileParams(this);
    }
}
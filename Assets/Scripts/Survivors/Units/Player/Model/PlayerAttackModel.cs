using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Weapon.Projectiles;
using UniRx;

namespace Survivors.Units.Player.Model
{
    public class PlayerAttackModel : IAttackModel
    {
        private readonly PlayerAttackConfig _config;
        private readonly FloatModifiableParameter _attackDamage;
        private readonly FloatModifiableParameter _attackTime;
        private readonly FloatModifiableParameter _projectileSpeed;
        private readonly FloatModifiableParameter _damageRadius;
        private readonly FloatModifiableParameter _damageAngle;
        private readonly FloatModifiableParameter _shotCount;   
        private readonly FloatModifiableParameter _attackDistance;

        public PlayerAttackModel(PlayerAttackConfig config, IModifiableParameterOwner parameterOwner)
        {
            _config = config;
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, _config.AttackDamage, parameterOwner);
            _attackTime = new FloatModifiableParameter(Parameters.ATTACK_TIME, _config.AttackTime, parameterOwner);
            _attackDistance = new FloatModifiableParameter(Parameters.ATTACK_DISTANCE, _config.AttackDistance, parameterOwner);
            
            _projectileSpeed = new FloatModifiableParameter(Parameters.PROJECTILE_SPEED, _config.ProjectileSpeed, parameterOwner);
            _damageRadius = new FloatModifiableParameter(Parameters.DAMAGE_RADIUS, _config.DamageRadius, parameterOwner);
            _damageAngle = new FloatModifiableParameter(Parameters.DAMAGE_ANGLE, _config.DamageAngle, parameterOwner);
            
            _shotCount = new FloatModifiableParameter(Parameters.SHOT_COUNT, 1, parameterOwner);
        }

        public float TargetSearchRadius => AttackDistance;
        public float AttackDistance => _attackDistance.Value;

        public float DamageRadius => _damageRadius.Value;

        public float DamageAngle => _damageAngle.Value;

        public float AttackDamage => _attackDamage.Value;

        public float ClipReloadTime => _config.ClipReloadTime;

        public IReadOnlyReactiveProperty<float> AttackTime => _attackTime.ReactiveValue;

        public float ProjectileSpeed => _projectileSpeed.Value;

        public int ClipSize => _config.ClipSize;
        public IReadOnlyReactiveProperty<int> ShotCount => _shotCount.ReactiveValue.Select(it => (int) it).ToReactiveProperty();

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams {
                    Speed = ProjectileSpeed,        
                    DamageRadius = DamageRadius,
                    DamageAngle = DamageAngle,
                    AttackDistance = AttackDistance,
                    Count = ShotCount.Value
            };
        }
    }
}
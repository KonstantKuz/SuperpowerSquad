using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Units.Model;
using Survivors.Units.Modifiers;
using Survivors.Units.Player.Config;
using Survivors.Units.Weapon.Projectiles;

namespace Survivors.Units.Player.Model
{
    public class PlayerAttackModel : IAttackModel
    {
        private readonly PlayerAttackConfig _config;
        private readonly FloatModifiableParameter _attackDamage;
        private readonly FloatModifiableParameter _attackTime;
        private readonly FloatModifiableParameter _projectileSpeed;
        private readonly FloatModifiableParameter _damageRadius;
        private readonly FloatModifiableParameter _shotCount;

        public PlayerAttackModel(PlayerAttackConfig config, IModifiableParameterOwner parameterOwner)
        {
            _config = config;
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, _config.AttackDamage, parameterOwner);
            _attackTime = new FloatModifiableParameter(Parameters.ATTACK_TIME, _config.AttackTime, parameterOwner);
            _projectileSpeed = new FloatModifiableParameter(Parameters.PROJECTILE_SPEED, _config.ProjectileSpeed, parameterOwner);
            _damageRadius = new FloatModifiableParameter(Parameters.DAMAGE_RADIUS, _config.DamageRadius, parameterOwner);
            _shotCount = new FloatModifiableParameter(Parameters.SHOT_COUNT, 1, parameterOwner);
        }

        public float TargetSearchRadius => AttackDistance;
        public float AttackDistance => _config.AttackDistance;

        public float DamageRadius => _damageRadius.Value;

        public float AttackDamage => _attackDamage.Value;

        public float ClipReloadTime => _config.ClipReloadTime;

        public float AttackTime => _attackTime.Value;

        public float ProjectileSpeed => _projectileSpeed.Value;

        public int ClipSize => _config.ClipSize;

        public int ShotCount => (int)_shotCount.Value;

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams {
                    Speed = ProjectileSpeed,        
                    DamageRadius = DamageRadius,
                    AttackDistance = AttackDistance,
                    Count = ShotCount
            };
        }
    }
}
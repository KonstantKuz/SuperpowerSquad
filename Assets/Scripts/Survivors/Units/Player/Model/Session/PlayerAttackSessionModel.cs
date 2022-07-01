using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;

namespace Survivors.Units.Player.Model.Session
{
    public class PlayerAttackSessionModel : IPlayerAttackModel
    {
        private readonly FloatModifiableParameter _attackDamage;
        private readonly FloatModifiableParameter _attackInterval;
        private readonly FloatModifiableParameter _projectileSpeed;
        private readonly FloatModifiableParameter _damageRadius;
        private readonly FloatModifiableParameter _attackDistance;

        public PlayerAttackSessionModel(IPlayerAttackModel model, IModifiableParameterOwner parameterOwner)
        {
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, model.AttackDamage, parameterOwner);
            _attackInterval = new FloatModifiableParameter(Parameters.ATTACK_INTERVAL, model.AttackInterval.Value, parameterOwner);
            _attackDistance = new FloatModifiableParameter(Parameters.ATTACK_DISTANCE, model.AttackDistance, parameterOwner);
            
            _projectileSpeed = new FloatModifiableParameter(Parameters.PROJECTILE_SPEED, model.ProjectileSpeed, parameterOwner);
            _damageRadius = new FloatModifiableParameter(Parameters.DAMAGE_RADIUS, model.DamageRadius, parameterOwner);
            
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
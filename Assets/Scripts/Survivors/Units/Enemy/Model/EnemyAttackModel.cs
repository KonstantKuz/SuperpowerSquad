using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyAttackModel : IAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            TargetSearchRadius = Mathf.Infinity;
            AttackDistance = config.AttackRange;
            AttackDamage = config.AttackDamage;
            AttackInterval = new ReactiveProperty<float>(config.AttackInterval);
            DamageRadius = config.DamageRadius;
            ProjectileSpeed = config.ProjectileSpeed;
        }

        public float TargetSearchRadius { get; }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }
        public float DamageRadius { get; }
        public float ProjectileSpeed { get; }

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams
            {
                AttackDistance = AttackDistance,
                DamageRadius = DamageRadius,
                Speed = ProjectileSpeed
            };
        }
    }
}
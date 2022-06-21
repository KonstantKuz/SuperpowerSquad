using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
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
            AttackTime = new ReactiveProperty<float>(config.AttackInterval);
        }

        public float TargetSearchRadius { get; }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public IReadOnlyReactiveProperty<float> AttackTime { get; }
    }
}
using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
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
            AttackInterval = config.AttackInterval;
        }

        public float TargetSearchRadius { get; }
        public float AttackDistance { get; }
        public int AttackDamage { get; }
        public float AttackInterval { get; }
    }
}
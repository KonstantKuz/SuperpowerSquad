using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyAttackModel : IAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            AttackDamage = config.AttackDamage;
            AttackInterval = config.AttackInterval;
            AttackDistance = config.AttackRange;
        }

        public float AttackDistance { get; }
        public int AttackDamage { get; }
        public float AttackInterval { get; }
    }
}
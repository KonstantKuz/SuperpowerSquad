using System.Runtime.Serialization;

namespace Survivors.Units.Enemy.Config
{
    [DataContract]
    public class EnemyAttackConfig
    {
        [DataMember(Name = "AttackRange")]
        private float _attackRange;
        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;

        public float AttackRange => _attackRange;
        public int AttackDamage => _attackDamage;
        public float AttackInterval => _attackInterval;
    }
}
using System.Runtime.Serialization;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class AttackConfig
    {
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "AttackRadius")]
        private float _attackRadius;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;

        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;

        [DataMember(Name = "AttackTime")]
        private float _attackTime;

        [DataMember(Name = "AttackSpeed")]
        private float _attackSpeed;

        [DataMember(Name = "AttackCount")]
        private int _attackCount;

        public float AttackDistance => _attackDistance;
        
        public float AttackRadius => _attackRadius;
        
        public int AttackDamage => _attackDamage;
        
        public float AttackInterval => _attackInterval;
        
        public float AttackTime => _attackTime;
        
        public float AttackSpeed => _attackSpeed;

        public int AttackCount => _attackCount;
    }
}
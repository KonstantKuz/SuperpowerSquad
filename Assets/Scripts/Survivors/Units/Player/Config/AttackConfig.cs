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

        [DataMember(Name = "RechargeTime")]
        private float _rechargeTime;

        [DataMember(Name = "AttackTime")]
        private float _attackTime;

        [DataMember(Name = "AttackSpeed")]
        private float _attackSpeed;

        [DataMember(Name = "ChargeCount")]
        private int _chargeCount;      
        
        [DataMember(Name = "AttackAngle")]
        private int _attackAngle;

        public float AttackDistance => _attackDistance;
        
        public float AttackRadius => _attackRadius;
        
        public int AttackDamage => _attackDamage;
        
        public float RechargeTime => _rechargeTime;
        
        public float AttackTime => _attackTime;
        
        public float AttackSpeed => _attackSpeed;

        public int ChargeCount => _chargeCount;
        public int AttackAngle => _attackAngle;
    }
}
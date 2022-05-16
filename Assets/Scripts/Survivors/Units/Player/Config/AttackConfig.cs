using System.Runtime.Serialization;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class AttackConfig
    {
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "DamageRadius")]
        private float _damageRadius;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;

        [DataMember(Name = "RechargeTime")]
        private float _rechargeTime;

        [DataMember(Name = "AttackTime")]
        private float _attackTime;

        [DataMember(Name = "ChargeSpeed")]
        private float _chargeSpeed;

        [DataMember(Name = "ChargeCount")]
        private int _chargeCount;      
        
        [DataMember(Name = "AttackAngle")]
        private int _attackAngle;

        public float AttackDistance => _attackDistance;
        
        public float DamageRadius => _damageRadius;
        
        public int AttackDamage => _attackDamage;
        
        public float RechargeTime => _rechargeTime;
        
        public float AttackTime => _attackTime;
        
        public float ChargeSpeed => _chargeSpeed;

        public int ChargeCount => _chargeCount;
        public int AttackAngle => _attackAngle;
    }
}
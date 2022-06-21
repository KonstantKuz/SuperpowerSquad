using System.Runtime.Serialization;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class PlayerAttackConfig
    {
        
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "DamageRadius")]
        private float _damageRadius;

        [DataMember(Name = "DamageAngle")]
        private float _damageAngle;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        
        [DataMember(Name = "AttackTime")]
        private float _attackTime;

        [DataMember(Name = "ProjectileSpeed")]
        private float _projectileSpeed;
        
        public float AttackDistance => _attackDistance;

        public float DamageRadius => _damageRadius;
        
        public float DamageAngle => _damageAngle;

        public int AttackDamage => _attackDamage;
        
        public float AttackTime => _attackTime;

        public float ProjectileSpeed => _projectileSpeed;
        
    }
}
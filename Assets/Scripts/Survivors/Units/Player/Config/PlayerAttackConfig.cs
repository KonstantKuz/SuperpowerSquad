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
        
        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        
        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;

        [DataMember(Name = "ProjectileSpeed")]
        private float _projectileSpeed;
        
        public float AttackDistance => _attackDistance;

        public float DamageRadius => _damageRadius;
        
        public int AttackDamage => _attackDamage;
        
        public float AttackInterval => _attackInterval;

        public float ProjectileSpeed => _projectileSpeed;
        
    }
}
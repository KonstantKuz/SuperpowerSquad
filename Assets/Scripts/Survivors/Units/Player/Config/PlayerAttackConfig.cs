using System.Runtime.Serialization;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class PlayerAttackConfig
    {
        private const int DEFAULT_CLIP_SIZE = 1;
        
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "DamageRadius")]
        private float _damageRadius;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;

        [DataMember(Name = "ClipReloadTime")]
        private float _clipReloadTime;

        [DataMember(Name = "AttackTime")]
        private float _attackTime;

        [DataMember(Name = "ProjectileSpeed")]
        private float _projectileSpeed;
        
        public float AttackDistance => _attackDistance;

        public float DamageRadius => _damageRadius;

        public int AttackDamage => _attackDamage;

        public float ClipReloadTime => _clipReloadTime;

        public float AttackTime => _attackTime;

        public float ProjectileSpeed => _projectileSpeed;

        public int ClipSize => DEFAULT_CLIP_SIZE;
    }
}
using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.FormationWeapon;
using Survivors.Units.Weapon.Projectiles.Params;

namespace Survivors.Units.Enemy.Config
{
    public class BossAttackConfig : ICollectionItem<ProjectileFormationType>
    {
        [DataMember(Name = "Id")]
        private ProjectileFormationType _id;
        public ProjectileFormationType Id => _id;

        [DataMember] 
        public int Count;
        [DataMember] 
        public float Interval;
        [DataMember] 
        public float Damage;
        [DataMember] 
        public int ShotCount;
        [DataMember] 
        public float DamageRadius;
        [DataMember]
        public float ProjectileSpeed;

        public IProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams
            {
                Count = ShotCount,
                DamageRadius = DamageRadius,
                Speed = ProjectileSpeed
            };
        }
    }
}
using Survivors.Units.Player.Config;
using Survivors.Units.Weapon.Projectile;

namespace Survivors.Units.Player.Model
{
    public class AttackModel
    {
        private readonly AttackConfig _config;

        public AttackModel(AttackConfig config)
        {
            _config = config;
        }

        public float AttackDistance => _config.AttackDistance;

        public float DamageRadius => _config.DamageRadius;

        public int AttackDamage => _config.AttackDamage;

        public float ClipReloadTime => _config.ClipReloadTime;

        public float AttackTime => _config.AttackTime;

        public float ProjectileSpeed => _config.ProjectileSpeed;

        public int ClipSize => _config.ClipSize;

        public ProjectileParams CreateProjectileParams()
        {
            return new ProjectileParams() {
                    Speed = ProjectileSpeed,        
                    DamageRadius = DamageRadius,
            };
        }
    }
}
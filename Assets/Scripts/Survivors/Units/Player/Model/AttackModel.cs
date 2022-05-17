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

        public float ClipReloadTime => _config.RechargeTime;

        public float AttackTime => _config.AttackTime;

        public float ProjectileSpeed => _config.ChargeSpeed;

        public int ClipSize => _config.ChargeCount;

        public ProjectileParams CreateChargeParams()
        {
            return new ProjectileParams() {
                    Speed = ProjectileSpeed,        
                    DamageRadius = DamageRadius,
            };
        }
    }
}
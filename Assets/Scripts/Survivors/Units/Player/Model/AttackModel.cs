using Survivors.Units.Player.Config;
using Survivors.Units.Weapon;
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

        public float RechargeTime => _config.RechargeTime;

        public float AttackTime => _config.AttackTime;

        public float ChargeSpeed => _config.ChargeSpeed;

        public int ChargeCount => _config.ChargeCount;    
        public int AttackAngle => _config.AttackAngle;
        
        public ProjectileParams CreateChargeParams()
        {
            return new ProjectileParams() {
                    Speed = ChargeSpeed,        
                    DamageRadius = DamageRadius,
            };
        }
    }
}
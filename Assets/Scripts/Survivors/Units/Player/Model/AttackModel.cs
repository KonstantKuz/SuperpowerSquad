using Survivors.Units.Player.Config;

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

        public float AttackRadius => _config.AttackRadius;

        public int AttackDamage => _config.AttackDamage;

        public float RechargeTime => _config.RechargeTime;

        public float AttackTime => _config.AttackTime;

        public float AttackSpeed => _config.AttackSpeed;

        public int ChargeCount => _config.ChargeCount;    
        public int AttackAngle => _config.AttackAngle;
    }
}
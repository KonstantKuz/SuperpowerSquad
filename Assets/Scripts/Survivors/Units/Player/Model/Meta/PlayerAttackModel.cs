using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using UniRx;

namespace Survivors.Units.Player.Model.Meta
{
    public interface IPlayerAttackModel : IAttackModel
    {
        float DamageRadius { get; }
        float ProjectileSpeed { get; }
    }

    public class PlayerAttackModel : IPlayerAttackModel
    {
        private readonly PlayerAttackConfig _config;
        
        public PlayerAttackModel(PlayerAttackConfig config)
        {
            _config = config;
            AttackInterval = new ReactiveProperty<float>(_config.AttackInterval);
        }

        public float TargetSearchRadius => AttackDistance;
        public float AttackDistance => _config.AttackDistance;

        public float DamageRadius => _config.DamageRadius;

        public float AttackDamage => _config.AttackDamage;
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }

        public float ProjectileSpeed => _config.ProjectileSpeed;
    }
}
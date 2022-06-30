using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using UniRx;

namespace Survivors.Units.Player.Model.Meta
{
    public interface IPlayerHealthModel : IHealthModel
    {
    }

    public class PlayerHealthModel : IPlayerHealthModel
    {
        private readonly PlayerUnitConfig _config;
        private readonly IReadOnlyReactiveProperty<float> _maxHealth;
        
        public PlayerHealthModel(PlayerUnitConfig config)
        {
            _config = config;
            _maxHealth = new ReactiveProperty<float>(_config.Health);
        }
        public float StartingMaxHealth => _config.Health;
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
    }
}
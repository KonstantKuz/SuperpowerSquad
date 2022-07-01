using Survivors.Units.Player.Config;
using UniRx;

namespace Survivors.Units.Player.Model.Meta
{
    public class PlayerHealthModel : IPlayerHealthModel
    {
        private readonly PlayerUnitConfig _config;
        private readonly IReadOnlyReactiveProperty<float> _maxHealth;
        
        public PlayerHealthModel(PlayerUnitConfig config)
        {
            _config = config;
            _maxHealth = new ReactiveProperty<float>(StartingMaxHealth);
        }
        public float StartingMaxHealth => _config.Health;
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
    }
}
using Survivors.Units.Model;
using Survivors.Units.Player.Config;

namespace Survivors.Units.Player.Model
{
    public class PlayerUnitModel : IUnitModel
    {
        private readonly PlayerUnitConfig _config;

        private readonly PlayerAttackModel _playerAttackModel;

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            _config = config;
            HealthModel = new HealthModel(config.Health);
            _playerAttackModel = new PlayerAttackModel(config.AttackConfig);
        }

        public string Id => _config.Id;
        public IUnitHealthModel HealthModel { get; }
        public IAttackModel AttackModel => _playerAttackModel;
    }
}
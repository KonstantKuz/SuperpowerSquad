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
            _playerAttackModel = new PlayerAttackModel(config.AttackConfig);
        }

        public IAttackModel AttackModel => _playerAttackModel;
        public string Id => _config.Id;
    }
}
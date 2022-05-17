using Survivors.Units.Player.Config;

namespace Survivors.Units.Player.Model
{
    public class PlayerUnitModel : IUnitModel
    {
        private readonly PlayerUnitConfig _config;

        private readonly AttackModel _attackModel;

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            _config = config;
            _attackModel = new AttackModel(config.AttackConfig);
        }

        public AttackModel AttackModel => _attackModel;
        public string Id => _config.Id;
    }
}
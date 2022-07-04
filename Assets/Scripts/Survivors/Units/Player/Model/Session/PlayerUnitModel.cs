using Feofun.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;

namespace Survivors.Units.Player.Model.Session
{
    public class PlayerUnitModel : ModifiableParameterOwner, IUnitModel
    {
        private readonly PlayerUnitConfig _config;
        private readonly PlayerAttackModel _playerAttackModel;

        public PlayerUnitModel(PlayerUnitConfig config, MetaParameterCalculator parameterCalculator)
        {
            _config = config;
            HealthModel = new PlayerHealthModel(config.Health, this);
            _playerAttackModel = new PlayerAttackModel(config.PlayerAttackConfig, this, parameterCalculator);
        }

        public string Id => _config.Id;
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel => _playerAttackModel;
    }
}
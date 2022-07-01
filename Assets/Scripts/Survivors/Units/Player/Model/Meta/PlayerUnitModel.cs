using Feofun.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;

namespace Survivors.Units.Player.Model.Meta
{
    public class PlayerUnitModel : ModifiableParameterOwner, IPlayerUnitModel
    {
        private readonly PlayerUnitConfig _config;

        public PlayerUnitModel(PlayerUnitConfig config, MetaParameterCalculator parameterCalculator)
        {
            _config = config;
            PlayerHealthModel = new PlayerHealthModel(config);
            PlayerAttackModel = new PlayerAttackModel(config.PlayerAttackConfig, this, parameterCalculator);
        }
        public string Id => _config.Id;
        public IPlayerHealthModel PlayerHealthModel { get; }
        public IPlayerAttackModel PlayerAttackModel { get; }
        public IHealthModel HealthModel => PlayerHealthModel;
        public IAttackModel AttackModel => PlayerAttackModel;
    }
}
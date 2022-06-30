using Survivors.Units.Model;
using Survivors.Units.Player.Config;

namespace Survivors.Units.Player.Model.Meta
{
    public interface IPlayerUnitModel : IUnitModel
    {
        string Id { get; }
        IPlayerHealthModel PlayerHealthModel { get; }
        IPlayerAttackModel PlayerAttackModel { get; }
    }

    public class PlayerUnitModel : IPlayerUnitModel
    {
        private readonly PlayerUnitConfig _config;

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            _config = config;
            PlayerHealthModel = new PlayerHealthModel(config);
            PlayerAttackModel = new PlayerAttackModel(config.PlayerAttackConfig);
        }

        public string Id => _config.Id;
        public IPlayerHealthModel PlayerHealthModel { get; }
        public IPlayerAttackModel PlayerAttackModel { get; }
        public IHealthModel HealthModel => PlayerHealthModel;
        public IAttackModel AttackModel => PlayerAttackModel;
    }
}
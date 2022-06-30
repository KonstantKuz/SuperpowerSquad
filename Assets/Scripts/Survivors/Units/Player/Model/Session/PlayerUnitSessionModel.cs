using Feofun.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Model.Meta;

namespace Survivors.Units.Player.Model.Session
{
    public class PlayerUnitSessionModel : ModifiableParameterOwner, IPlayerUnitModel
    {
        private readonly PlayerUnitModel _base;

        public PlayerUnitSessionModel(PlayerUnitModel unit)
        {
            _base = unit;
            PlayerHealthModel = new PlayerHealthSessionModel(unit.PlayerHealthModel, this);
            PlayerAttackModel = new PlayerAttackSessionModel(unit.PlayerAttackModel, this);
        }

        public string Id => _base.Id;
        public IPlayerHealthModel PlayerHealthModel { get; }
        public IPlayerAttackModel PlayerAttackModel { get; }
        public IHealthModel HealthModel => PlayerHealthModel;
        public IAttackModel AttackModel => PlayerAttackModel;
    }
}
using Survivors.Units.Model;

namespace Survivors.Units.Player.Model
{
    public interface IPlayerUnitModel : IUnitModel
    {
        IPlayerHealthModel PlayerHealthModel { get; }
        IPlayerAttackModel PlayerAttackModel { get; }
    }
}
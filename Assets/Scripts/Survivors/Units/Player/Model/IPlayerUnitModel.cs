using Survivors.Units.Model;

namespace Survivors.Units.Player.Model
{
    public interface IPlayerUnitModel : IUnitModel
    {
        string Id { get; }
        IPlayerHealthModel PlayerHealthModel { get; }
        IPlayerAttackModel PlayerAttackModel { get; }
    }
}
using Survivors.Units.Model;

namespace Survivors.Units
{
    public interface IUnitModel
    {
        string Id { get; }
        IUnitHealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
    }
}
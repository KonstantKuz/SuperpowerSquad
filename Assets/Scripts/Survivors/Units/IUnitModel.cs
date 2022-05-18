using Survivors.Units.Model;

namespace Survivors.Units
{
    public interface IUnitModel
    {
        string Id { get; }
        HealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
    }
}
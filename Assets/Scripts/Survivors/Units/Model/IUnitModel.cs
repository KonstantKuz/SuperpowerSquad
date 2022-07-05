
namespace Survivors.Units.Model
{
    public interface IUnitModel
    {
        string Id { get; }
        IHealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
    }
}
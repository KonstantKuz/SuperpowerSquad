namespace Survivors.Units.Model
{
    public interface IUnitModel
    {
        IAttackModel AttackModel { get; }
        string Id { get; }
    }
}
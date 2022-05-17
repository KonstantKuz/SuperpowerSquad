namespace Survivors.Units.Player.Model
{
    public interface IUnitModel
    {
        string Id { get; }
        AttackModel AttackModel { get; }
    }
}
namespace Survivors.Units.Model
{
    public interface IUnitHealthModel
    {
        int MaxHealth { get; }
        int StartingHealth { get; }
    }
}
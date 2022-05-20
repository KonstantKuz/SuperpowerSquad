using Feofun.Modifiers;

namespace Survivors.Units.Model
{
    public interface IUnitModel
    {
        string Id { get; }
        HealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
        void AddModifier(IModifier modifier);
    }
}
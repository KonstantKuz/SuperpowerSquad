using Feofun.Modifiers;
using Survivors.Units.Enemy.Model;

namespace Survivors.Units.Model
{
    public interface IUnitModel
    {
        string Id { get; }
        IHealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
        void AddModifier(IModifier modifier);
    }
}
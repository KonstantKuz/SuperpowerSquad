namespace Survivors.Units.Model
{
    public interface IAttackModel
    {
        float TargetSearchRadius { get; }
        float AttackDistance { get; }
        int AttackDamage { get; } 
    }
}
namespace Survivors.Units.Weapon.Projectiles.Params
{
    public interface IProjectileParams
    {
        float Speed { get; }
        float DamageRadius { get; }
        float AttackDistance { get; }
        public float DamageAngle { get; }
        int Count { get; }
    }
}
namespace Survivors.Units.Weapon.Projectiles.Params
{
    public class ProjectileParams : IProjectileParams
    {
        public float Speed { get; set; }
        public float DamageRadius { get; set; }
        public float AttackDistance { get; set; }
        public int Count { get; set; }
    }
}
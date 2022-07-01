using Survivors.Units.Model;

namespace Survivors.Units.Player.Model
{
    public interface IPlayerAttackModel : IAttackModel
    {
        float DamageRadius { get; }
        float ProjectileSpeed { get; }
    }
}
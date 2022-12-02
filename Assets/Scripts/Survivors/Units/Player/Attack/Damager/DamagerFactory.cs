using Survivors.Units.Player.Model;
using Survivors.Units.Weapon;

namespace Survivors.Units.Player.Attack.Damager
{
    public class DamagerFactory
    {
        public static IDamager CreateForPlayer(PlayerAttackModel attackModel, BaseWeapon weapon)
        {
            if (weapon is QueueWeapon queueWeapon)
            {
                return new QueueWeaponDamager(attackModel, queueWeapon);
            }

            return new PlayerDamager(attackModel);
        }
    }
}
using UnityEngine;

namespace Survivors.Units.Player.Attack.Damager
{
    public interface IDamager
    {
        void Damage(GameObject target);
    }
}
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public interface IDamager
    {
        void Damage(GameObject target);
    }
}
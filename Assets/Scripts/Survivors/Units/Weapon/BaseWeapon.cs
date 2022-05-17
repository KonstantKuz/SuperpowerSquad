using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectile;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}
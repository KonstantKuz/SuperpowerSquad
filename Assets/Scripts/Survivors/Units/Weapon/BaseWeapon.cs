using System;
using JetBrains.Annotations;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}
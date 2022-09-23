using System;
using System.Collections;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public interface IFireFormation
    {
        IEnumerator Fire(Func<Projectile> createProjectile, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}
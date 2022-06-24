using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LightningStrike : Projectile
    {
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            
            transform.position = target.Center.position;
            transform.localScale *= projectileParams.DamageRadius;
            TryHit(target.Root.parent.gameObject, target.Center.position, Vector3.zero);
        }
    }
}
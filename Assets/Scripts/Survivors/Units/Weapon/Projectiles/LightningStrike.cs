using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LightningStrike : Projectile
    {
        private Transform _targetCenter;
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);

            _targetCenter = target.Center;
            transform.position = _targetCenter.position;
            transform.localScale *= projectileParams.DamageRadius;
            TryHit(target.Root.parent.gameObject, target.Center.position, Vector3.zero);
        }

        private void Update()
        {
            if(_targetCenter == null) return;
            transform.position = _targetCenter.position;
        }
    }
}
using System;
using System.Collections;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class Volley : IFireFormation
    {
        private readonly Transform _barrel;
        private readonly float _subInterval;

        public Volley(Transform barrel, float subInterval)
        {
            _barrel = barrel;
            _subInterval = subInterval;
        }
        
        public IEnumerator Fire(Func<Projectile> createProjectile, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = createProjectile.Invoke();
                var shootRotation = RangedWeapon.GetShootRotation(_barrel.position, target.Center.position, true);
                projectile.transform.SetPositionAndRotation(_barrel.position, shootRotation);
                projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
                projectile.Launch(target, projectileParams, hitCallback);
                yield return new WaitForSeconds(_subInterval);
            }
        }
    }
}
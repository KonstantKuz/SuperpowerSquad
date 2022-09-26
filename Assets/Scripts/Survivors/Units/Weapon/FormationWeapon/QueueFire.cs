using System;
using System.Collections;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class QueueFire : IFireFormation
    {
        private readonly Func<Projectile> _createProjectile;
        private readonly Transform _barrel;
        private readonly float _subInterval;

        public QueueFire(Func<Projectile> createProjectile, Transform barrel, float subInterval)
        {
            _createProjectile = createProjectile;
            _barrel = barrel;
            _subInterval = subInterval;
        }
        
        public IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = _createProjectile.Invoke();
                var shootRotation = RangedWeapon.GetShootRotation(_barrel.position, target.Center.position, true);
                projectile.transform.SetPositionAndRotation(_barrel.position, shootRotation);
                projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
                projectile.Launch(target, projectileParams, hitCallback);
                yield return new WaitForSeconds(_subInterval);
            }
        }
    }
}
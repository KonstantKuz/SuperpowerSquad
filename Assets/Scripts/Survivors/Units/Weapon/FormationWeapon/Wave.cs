using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class Wave : IFireFormation
    {
        private readonly Transform _barrel;
        private readonly int _attackNumber;
        private readonly float _initialRadius;

        public Wave(Transform barrel, int attackNumber, float initialRadius)
        {
            _barrel = barrel;
            _attackNumber = attackNumber;
            _initialRadius = initialRadius;
        }
        
        public IEnumerator Fire(Func<Projectile> createProjectile, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var angleBtwnProjectiles = 360f / projectileParams.Count;
            var currentAngle = angleBtwnProjectiles / 2 * _attackNumber;
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = createProjectile.Invoke();
                projectile.transform.position = _barrel.transform.position + Quaternion.Euler(0, currentAngle, 0) * _barrel.transform.forward * _initialRadius;
                projectile.transform.forward = (projectile.transform.position - _barrel.transform.position).XZ();
                projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
                projectile.Launch(target, projectileParams, hitCallback);
                currentAngle += angleBtwnProjectiles;
            }
            yield break;
        }
    }
}
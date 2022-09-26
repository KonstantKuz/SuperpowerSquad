using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class CircleFire : IFireFormation
    {
        private readonly Func<Projectile> _createProjectile;
        private readonly Transform _barrel;
        private readonly int _attackNumber;
        private readonly float _initialRadius;

        public CircleFire(Func<Projectile> createProjectile, Transform barrel, int attackNumber, float initialRadius)
        {
            _createProjectile = createProjectile;
            _barrel = barrel;
            _attackNumber = attackNumber;
            _initialRadius = initialRadius;
        }
        
        public IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var angleBtwnProjectiles = 360f / projectileParams.Count;
            var currentAngle = angleBtwnProjectiles / 2 * _attackNumber;
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = _createProjectile.Invoke();
                projectile.transform.position = _barrel.transform.position + Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _initialRadius;
                projectile.transform.forward = (projectile.transform.position - _barrel.transform.position).XZ();
                projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
                projectile.Launch(target, projectileParams, hitCallback);
                currentAngle += angleBtwnProjectiles;
            }
            yield break;
        }
    }
}
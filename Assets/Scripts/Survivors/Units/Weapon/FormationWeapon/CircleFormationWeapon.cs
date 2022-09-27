using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class CircleFormationWeapon : WeaponWithFormation
    {
        [SerializeField] private float _initialRadius;
        
        private int _attackNumber;
        
        public override IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _attackNumber++;
            var angleBtwnProjectiles = 360f / projectileParams.Count;
            var currentAngle = angleBtwnProjectiles / 2 * _attackNumber;
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = CreateProjectile();
                projectile.transform.position = Barrel.transform.position + Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _initialRadius;
                projectile.transform.forward = (projectile.transform.position - Barrel.transform.position).XZ();
                projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
                projectile.Launch(target, projectileParams, hitCallback);
                currentAngle += angleBtwnProjectiles;
            }
            yield break;
        }
    }
}
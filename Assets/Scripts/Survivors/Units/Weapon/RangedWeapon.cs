using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private bool _aimInXZPlane;
        [SerializeField]
        private bool _supportShotCount = true;
        [SerializeField]
        private Projectile _ammo;
        [SerializeField]
        private float _angleBetweenShots;
        [Inject]
        protected WorldObjectFactory ObjectFactory;
        
        
        protected Vector3 BarrelPos; //Seems that in some cases unity cannot correctly take position inside animation event
        
        protected bool AimInXZPlane => _aimInXZPlane;
        protected Projectile Ammo => _ammo;
        protected float AngleBetweenShots => _angleBetweenShots;

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var rotationToTarget = GetShootRotation(BarrelPos, target.Center.position, _aimInXZPlane);
            if (!_supportShotCount)
            {
                FireSingleShot(rotationToTarget, target, projectileParams, hitCallback);
            }
            else
            {
                FireMultipleShots(rotationToTarget, target, projectileParams, hitCallback);
            }
        }
        
        private void FireSingleShot(Quaternion rotation, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var projectile = CreateProjectile();
            projectile.transform.SetPositionAndRotation(BarrelPos, rotation);
            projectile.Launch(target, projectileParams, hitCallback);
        }

        private void FireMultipleShots(Quaternion rotationToTarget, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var spreadAngles = GetSpreadInAngle(projectileParams.Count).ToList();
            for (int i = 0; i < projectileParams.Count; i++) {
                var rotation = rotationToTarget * Quaternion.Euler(0, spreadAngles[i], 0);
                FireSingleShot(rotation, target, projectileParams, hitCallback);
            }
        }

        private IEnumerable<float> GetSpreadInAngle(int count)
        {
            for (int i = 0; i < count; i++) {
                yield return _angleBetweenShots * (2 * i + 1 - count) / 2;
            }
        }

        public static Quaternion GetShootRotation(Vector3 shootPos, Vector3 targetPos, bool aimInXZPlane)
        {
            var shootDirection = GetShootDirection(shootPos, targetPos);
            if (aimInXZPlane) {
                shootDirection = shootDirection.XZ();
            }
            return Quaternion.LookRotation(shootDirection);
        }

        protected static Vector3 GetShootDirection(Vector3 shootPos, Vector3 targetPos)
        {
            var dir = targetPos - shootPos;
            return dir.normalized;
        }

        private Projectile CreateProjectile()
        {
            return ObjectFactory.CreateObject(_ammo.gameObject).GetComponent<Projectile>();
        }

        private void LateUpdate()
        {
            BarrelPos = _barrel.position;
        }
    }
}
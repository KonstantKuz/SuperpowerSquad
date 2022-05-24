using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
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
        private Projectile _ammo;

        private Vector3 _barrelPos; //Seems that in some cases unity cannot correctly take position inside animation event

        [SerializeField] 
        private float _angleBetweenShots;
        
        [Inject]
        private WorldObjectFactory _objectFactory;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var rotationToTarget = GetShootRotation(_barrelPos, target.Center.position, _aimInXZPlane);
            var spreadAngles = GetSpreadInAngle(projectileParams.Count).ToList();
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = CreateProjectile();
                var rotation = rotationToTarget * Quaternion.Euler(0, spreadAngles[i], 0);
                projectile.transform.SetPositionAndRotation(_barrelPos, rotation);
                projectile.Launch(target, projectileParams, hitCallback);
            }
        }

        private IEnumerable<float> GetSpreadInAngle(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return _angleBetweenShots * (2 * i + 1 - count)/2; 
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

        private static Vector3 GetShootDirection(Vector3 shootPos, Vector3 targetPos)
        {
            var dir = targetPos - shootPos;
            return dir.normalized;
        }

        private Projectile CreateProjectile()
        {
            return _objectFactory.CreateObject(_ammo.gameObject).GetComponent<Projectile>();
        }

        private void LateUpdate()
        {
            _barrelPos = _barrel.position;
        }
    }
}
using System;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
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

        [Inject]
        private WorldObjectFactory _objectFactory;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var projectile = CreateProjectile();
            var rotationToTarget = GetShootRotation(_barrelPos, target.Center.position, _aimInXZPlane);
            projectile.transform.SetPositionAndRotation(_barrelPos, rotationToTarget);
            projectile.Launch(target, projectileParams, hitCallback);
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
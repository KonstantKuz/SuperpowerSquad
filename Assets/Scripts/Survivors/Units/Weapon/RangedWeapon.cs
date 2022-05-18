using System;
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
        private Projectile _ammo;
        [Inject]
        private WorldObjectFactory _objectFactory;

        private Vector3 _barrelPos; //Seems that in some cases unity cannot correctly take position inside animation event

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var projectile = CreateProjectile();
            var rotationToTarget = GetShootRotation(_barrelPos, target.Center.position);
            projectile.transform.SetPositionAndRotation(_barrelPos, rotationToTarget);
            projectile.Launch(target, projectileParams, hitCallback);
        }

        private static Quaternion GetShootRotation(Vector3 shootPos, Vector3 targetPos)
        {
            return Quaternion.LookRotation(GetShootDirection(shootPos, targetPos));
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
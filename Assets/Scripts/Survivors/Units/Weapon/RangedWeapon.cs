using System;
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
        private Projectile _ammo;

        private Vector3 _barrelPos; //Seems that in some cases unity cannot correctly take position inside animation event

        [SerializeField] 
        private float _angleBetweenShots;
        
        [Inject]
        private WorldObjectFactory _objectFactory;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var rotationToTarget = GetShootRotation(_barrelPos, target.Center.position);
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var projectile = CreateProjectile();
                var rotation = rotationToTarget * Quaternion.Euler(0, _angleBetweenShots * (0.5f - 0.5f * projectileParams.Count + i), 0);
                projectile.transform.SetPositionAndRotation(_barrelPos, rotation);
                projectile.Launch(target, projectileParams, hitCallback);
            }
        }

        public static Quaternion GetShootRotation(Vector3 shootPos, Vector3 targetPos)
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
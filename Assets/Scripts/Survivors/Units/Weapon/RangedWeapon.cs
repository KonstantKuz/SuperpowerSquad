using System;
using Survivors.GameWorld.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Charge.Projectile;
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
    
        public override void Fire(ITarget target, ChargeParams chargeParams, Action<GameObject> hitCallback)
        {
            var projectile = CreateProjectile();
            var pos = _barrel.position;
            var rotationToTarget = GetShootRotation(pos, target.Center.position);
            projectile.transform.SetPositionAndRotation(pos, rotationToTarget);
            projectile.Launch(target, chargeParams, hitCallback);
        }
        
        private static Quaternion GetShootRotation(Vector3 shootPos, Vector3 targetPos)
        {
            return Quaternion.LookRotation(GetShootDirection(shootPos, targetPos));
        }
        private static Vector3 GetShootDirection(Vector3 shootPos, Vector3 targetPos)
        {
            var dir = targetPos - shootPos;
            dir = new Vector3(dir.x, 0, dir.z);
            return dir.normalized;
        }
        private Projectile CreateProjectile()
        {
            return _objectFactory.CreateObject(_ammo.gameObject).GetComponent<Projectile>();
        }
    }
}
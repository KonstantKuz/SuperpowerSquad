using System;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Weapon
{
    public class MeteorWeapon : RangedWeapon
    {
        [SerializeField] private float _startHeight; 
            
        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var projectile = ObjectFactory.CreateObject(Ammo.gameObject).RequireComponent<Bullet>();
            projectile.transform.SetPositionAndRotation(
                target.Root.position + _startHeight * Vector3.up,
                Quaternion.LookRotation(Vector3.down));
            projectile.SetMaxLifeTime(_startHeight / projectileParams.Speed);

            var modifiedParams = new ProjectileParams
            {
                AttackDistance = _startHeight,
                Count = 1,
                DamageAngle = 0,
                DamageRadius = projectileParams.DamageRadius,
                Speed = projectileParams.Speed
            };
            projectile.Launch(target, modifiedParams, hitCallback);
        }
    }
}
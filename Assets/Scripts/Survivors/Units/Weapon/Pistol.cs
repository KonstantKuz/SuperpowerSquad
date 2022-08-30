using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class Pistol : RangedWeapon
    {
        private const float DAMAGE_RADIUS = 1f;
        [SerializeField] private float _bulletSpeed;
        
        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            projectileParams ??= CreateDefaultParams();
            base.Fire(target, projectileParams, hitCallback);
        }

        private ProjectileParams CreateDefaultParams()
        {
            return new ProjectileParams
            {
                Speed = _bulletSpeed,
                DamageRadius = DAMAGE_RADIUS,
            };
        }
    }
}
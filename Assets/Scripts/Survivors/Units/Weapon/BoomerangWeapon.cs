using System;
using System.Collections.Generic;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class BoomerangWeapon : RangedWeapon
    {
        private readonly List<Projectile> _boomerangs = new List<Projectile>();

        public override bool CanFire => _boomerangs.Count == 0;

        protected override void Fire(Quaternion rotation, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var boomerang = CreateBoomerang();
            boomerang.transform.SetPositionAndRotation(BarrelPos, rotation);
            boomerang.Launch(Barrel, target, projectileParams, hitCallback, OnDestroyBoomerang);
            _boomerangs.Add(boomerang);
        }

        private void OnDestroyBoomerang(Boomerang boomerang)
        {
            _boomerangs.Remove(boomerang);
        }

        private Boomerang CreateBoomerang()
        {
            return ObjectFactory.CreateObject(Ammo.gameObject).GetComponent<Boomerang>();
        }
    }
}
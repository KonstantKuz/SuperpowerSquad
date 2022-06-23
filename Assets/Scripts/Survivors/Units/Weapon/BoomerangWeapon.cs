using System;
using System.Collections.Generic;
using Survivors.Units.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class BoomerangWeapon : RangedWeapon, IWeaponTimerManager
    {
        private readonly List<Projectile> _boomerangs = new List<Projectile>();
        private WeaponTimer _timer;
        
        public void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady)
        {
            _timer = new WeaponTimer(attackModel.AttackInterval);
            _timer.OnAttackReady += onAttackReady;
        }
        public void Unsubscribe(string weaponId, Action onAttackReady)
        {
            _timer.OnAttackReady -= onAttackReady;
        }

        private void Update()
        {
            if (_boomerangs.Count == 0)
            {
                _timer.OnTick();
            }
        }

        protected override void FireSingleShot(Quaternion rotation, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
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
﻿using System;
using System.Collections;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class QueueFormationWeapon : WeaponWithFormation
    {
        [SerializeField] private float _subInterval;
        
        public override IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            for (int i = 0; i < projectileParams.Count; i++)
            {
                var shootRotation = RangedWeapon.GetShootRotation(Barrel.position, target.Center.position, true);
                LaunchProjectile(Barrel.position, shootRotation, target, projectileParams, hitCallback);
                yield return new WaitForSeconds(_subInterval);
            }
        }
    }
}
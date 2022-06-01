using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Units.Weapon
{
    public class BombWeapon : RangedWeapon
    {
        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        { 
            Assert.IsNotNull(projectileParams);
            var spreadAngles = GetSpreadInAngle(projectileParams.Count).ToList();
            
            for (int i = 0; i < projectileParams.Count; i++) {
                var targetPos = target.Center.position;
                if (!spreadAngles[i].IsZero()) {
                    targetPos = GetTargetPositionOfSpreadAngle(target.Center.position, spreadAngles[i], projectileParams.DamageRadius);
                }
                Fire(targetPos, target, projectileParams, hitCallback);
            }
        }
        
        private IEnumerable<float> GetSpreadInAngle(int count)
        {
            var halfSumOfAngles = AngleBetweenShots * (int) Math.Ceiling((decimal) count / 2);
            for (int i = 1; i <= count; i++) {
                yield return AngleBetweenShots * i - halfSumOfAngles;
            }
        }
        private void Fire(Vector3 targetPos, ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var bomb = CreateBomb();
            var rotation = GetShootRotation(BarrelPos, targetPos, AimInXZPlane);
            bomb.transform.SetPositionAndRotation(BarrelPos, rotation);
            bomb.Launch(target, projectileParams, hitCallback, targetPos);
        }

        private Vector3 GetTargetPositionOfSpreadAngle(Vector3 targetPosition, float spreadAngle, float damageRadius)
        {
            var spreadedDirection = Quaternion.Euler(0, spreadAngle, 0) * GetShootDirection(BarrelPos, targetPosition);
            var randomDistance = Random.Range(damageRadius, damageRadius * 2);
            return targetPosition + (spreadedDirection * randomDistance);
        }

        private Bomb CreateBomb()
        {
            return ObjectFactory.CreateObject(Ammo.gameObject).RequireComponent<Bomb>();
        }
    }
}
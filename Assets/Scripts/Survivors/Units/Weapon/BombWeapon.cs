using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Units.Weapon
{
    public class BombWeapon : RangedWeapon
    {
        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        { 
            Assert.IsNotNull(projectileParams);
            var spreadAngles = GetSpreadInAngle(projectileParams.Count).ToList();
            
            for (int i = 0; i < projectileParams.Count; i++) {
                var targetPos = target.Center.position;
                if (!spreadAngles[i].IsZero()) {
                    targetPos = GetShootPosition(target.Center.position, spreadAngles[i], projectileParams.DamageRadius);
                }
                Fire(targetPos, target, projectileParams, hitCallback);
            }
        }
        
        private IEnumerable<float> GetSpreadInAngle(int count)
        {
            var halfCount = (int) Math.Ceiling((decimal) count / 2);
            foreach (var step in Enumerable.Range(-halfCount + 1, count))
            {
                yield return AngleBetweenShots * step;
            }
        }
        private void Fire(Vector3 targetPos, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var bomb = CreateBomb();
            var rotation = GetShootRotation(BarrelPos, targetPos, AimInXZPlane);
            bomb.transform.SetPositionAndRotation(BarrelPos, rotation);
            bomb.Launch(target, projectileParams, hitCallback, targetPos);
        }

        private Vector3 GetShootPosition(Vector3 targetPosition, float spreadAngle, float damageRadius)
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
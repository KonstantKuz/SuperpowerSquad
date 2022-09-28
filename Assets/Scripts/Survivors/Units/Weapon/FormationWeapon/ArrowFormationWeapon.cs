using System;
using System.Collections;
using Feofun.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class ArrowFormationWeapon : WeaponWithFormation
    {
        [SerializeField] private float _width;
        [SerializeField] private float _length;
        [SerializeField] private float _spreadAngle;
        
        public override IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var widthStep = _width / projectileParams.Count;
            var forward = Barrel.forward.XZ();
            
            LaunchProjectile(Barrel.position, Quaternion.LookRotation(forward), target, projectileParams, hitCallback);
            
            var subWaveCount = (projectileParams.Count - 1) / 2;
            var subLength = _length / subWaveCount;
            var subInterval = subLength / projectileParams.Speed;
            for (int i = 1; i < subWaveCount + 1; i++)
            {
                yield return new WaitForSeconds(subInterval);
                var leftProjectilePosition = Barrel.position + i * widthStep * Barrel.right;
                var leftProjectileForward = Quaternion.Euler(0, _spreadAngle * i, 0) * forward;
                LaunchProjectile(leftProjectilePosition, Quaternion.LookRotation(leftProjectileForward), target, projectileParams, hitCallback);
                var rightProjectilePosition = Barrel.position - i * widthStep * Barrel.right;
                var rightProjectileForward = Quaternion.Euler(0, -_spreadAngle * i, 0) * forward;
                LaunchProjectile(rightProjectilePosition, Quaternion.LookRotation(rightProjectileForward), target, projectileParams, hitCallback);
            }
        }
    }
}
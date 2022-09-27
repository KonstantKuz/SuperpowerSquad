using System;
using System.Collections;
using Survivors.Extension;
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
        
        private ITarget _target;
        private IProjectileParams _projectileParams;
        private Action<GameObject> _hitCallback;
        
        public override IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _target = target;
            _projectileParams = projectileParams;
            _hitCallback = hitCallback;

            var widthStep = _width / projectileParams.Count;
            var forward = Barrel.forward.XZ();
            
            LaunchProjectile(Barrel.position, forward);
            
            var subWaveCount = (projectileParams.Count - 1) / 2;
            var subLength = _length / subWaveCount;
            var subInterval = subLength / projectileParams.Speed;
            for (int i = 1; i < subWaveCount + 1; i++)
            {
                yield return new WaitForSeconds(subInterval);
                var leftProjectilePosition = Barrel.position + i * widthStep * Barrel.right;
                var rightProjectilePosition = Barrel.position - i * widthStep * Barrel.right;
                LaunchProjectile(leftProjectilePosition, Quaternion.Euler(0, _spreadAngle * i, 0) * forward);
                LaunchProjectile(rightProjectilePosition, Quaternion.Euler(0, -_spreadAngle * i, 0) * forward);
            }
        }

        private void LaunchProjectile(Vector3 position, Vector3 forward)
        {
            var projectile = CreateProjectile();
            projectile.transform.SetPositionAndRotation(position, Quaternion.LookRotation(forward));
            projectile.transform.localScale = Vector3.one * _projectileParams.DamageRadius;
            projectile.Launch(_target, _projectileParams, _hitCallback);
        }
    }
}
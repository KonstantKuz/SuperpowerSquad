using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class ArrowFire : IFireFormation
    {
        private readonly Func<Projectile> _createProjectile;
        private readonly Transform _barrel;
        private readonly float _width;
        private readonly float _length;
        private readonly float _spreadAngle;

        private ITarget _target;
        private IProjectileParams _projectileParams;
        private Action<GameObject> _hitCallback;

        public ArrowFire(Func<Projectile> createProjectile, Transform barrel, float width, float length, float spreadAngle)
        {
            _createProjectile = createProjectile;
            _barrel = barrel;
            _width = width;
            _length = length;
            _spreadAngle = spreadAngle;
        }
        
        public IEnumerator Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _target = target;
            _projectileParams = projectileParams;
            _hitCallback = hitCallback;

            var widthStep = _width / projectileParams.Count;
            var forward = _barrel.forward.XZ();
            
            LaunchProjectile(_barrel.position, forward);
            
            var subWaveCount = (projectileParams.Count - 1) / 2;
            var subLength = _length / subWaveCount;
            var subInterval = subLength / projectileParams.Speed;
            for (int i = 1; i < subWaveCount + 1; i++)
            {
                yield return new WaitForSeconds(subInterval);
                var leftProjectilePosition = _barrel.position + i * widthStep * _barrel.right;
                var rightProjectilePosition = _barrel.position - i * widthStep * _barrel.right;
                LaunchProjectile(leftProjectilePosition, Quaternion.Euler(0, _spreadAngle * i, 0) * forward);
                LaunchProjectile(rightProjectilePosition, Quaternion.Euler(0, -_spreadAngle * i, 0) * forward);
            }
        }

        private void LaunchProjectile(Vector3 position, Vector3 forward)
        {
            var projectile = _createProjectile.Invoke();
            projectile.transform.SetPositionAndRotation(position, Quaternion.LookRotation(forward));
            projectile.transform.localScale = Vector3.one * _projectileParams.DamageRadius;
            projectile.Launch(_target, _projectileParams, _hitCallback);
        }
    }
}
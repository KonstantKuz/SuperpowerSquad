using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class Wedge : IFireFormation
    {
        private readonly Transform _barrel;
        private readonly float _wedgeWidth;
        private readonly float _wedgeLength;

        private Func<Projectile> _createProjectile;
        private ITarget _target;
        private IProjectileParams _projectileParams;
        private Action<GameObject> _hitCallback;

        public Wedge(Transform barrel, float wedgeWidth, float wedgeLength)
        {
            _barrel = barrel;
            _wedgeWidth = wedgeWidth;
            _wedgeLength = wedgeLength;
        }
        
        public IEnumerator Fire(Func<Projectile> createProjectile, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _createProjectile = createProjectile;
            _target = target;
            _projectileParams = projectileParams;
            _hitCallback = hitCallback;

            var widthStep = _wedgeWidth / projectileParams.Count;
            var forward = _barrel.forward.XZ();
            
            LaunchProjectile(_barrel.position, forward);
            
            var subWaveCount = (projectileParams.Count - 1) / 2;
            var subLength = _wedgeLength / subWaveCount;
            var subInterval = subLength / projectileParams.Speed;
            for (int i = 1; i < subWaveCount + 1; i++)
            {
                yield return new WaitForSeconds(subInterval);
                var leftProjectilePosition = _barrel.position + i * widthStep * _barrel.right;
                var rightProjectilePosition = _barrel.position - i * widthStep * _barrel.right;
                LaunchProjectile(leftProjectilePosition, forward);
                LaunchProjectile(rightProjectilePosition, forward);
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
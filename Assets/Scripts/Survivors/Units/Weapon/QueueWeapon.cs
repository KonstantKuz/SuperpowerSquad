using System;
using System.Collections;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class QueueWeapon : RangedWeapon
    {
        [SerializeField] private int _queueSize;
        [SerializeField] private float _subInterval;

        private Coroutine _fireCoroutine;

        public int QueueSize => _queueSize;

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _fireCoroutine = StartCoroutine(FireQueue(target, projectileParams, hitCallback));
        }

        private IEnumerator FireQueue(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            for (int i = 0; i < _queueSize; i++)
            {
                var rotationToTarget = GetShootRotation(BarrelPos, target.Center.position, AimInXZPlane);
                FireSingleShot(rotationToTarget, target, projectileParams, hitCallback);
                yield return new WaitForSecondsRealtime(_subInterval);
            }
        }

        private void OnDisable()
        {
            StopFire();
        }

        private void StopFire()
        {
            if (_fireCoroutine != null)
            {
                StopCoroutine(_fireCoroutine);
                _fireCoroutine = null;
            }   
        }
    }
}
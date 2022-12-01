using System;
using System.Collections;
using ModestTree;
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
            Assert.IsNotNull(projectileParams);
            var rotationToTarget = GetShootRotation(BarrelPos, target.Center.position, AimInXZPlane);
            Action singleFireAction = () => FireSingleShot(rotationToTarget, target, projectileParams, hitCallback);
            Action multipleFireAction = () => FireMultipleShots(rotationToTarget, target, projectileParams, hitCallback);

            _fireCoroutine = SupportShotCount
                ? StartCoroutine(FireQueue(singleFireAction))
                : StartCoroutine(FireQueue(multipleFireAction));
        }

        private IEnumerator FireQueue(Action fire)
        {
            for (int i = 0; i < _queueSize; i++)
            {
                fire?.Invoke();
                yield return WaitForFixedSubInterval();
            }
        }

        private IEnumerator WaitForFixedSubInterval()
        {
            var fixedFramesCount = Mathf.RoundToInt(_subInterval / Time.fixedDeltaTime);
            for (int i = 0; i < fixedFramesCount; i++)
            {
                yield return new WaitForFixedUpdate();
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
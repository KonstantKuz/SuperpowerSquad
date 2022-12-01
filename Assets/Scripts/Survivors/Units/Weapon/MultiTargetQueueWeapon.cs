using System;
using System.Collections;
using System.Linq;
using Feofun.Extension;
using ModestTree;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class MultiTargetQueueWeapon : RangedWeapon
    {
        [SerializeField] private float _subInterval;

        private ITargetSearcher _targetSearcher;
        private Coroutine _fireCoroutine;

        private void Awake()
        {
            _targetSearcher = gameObject.RequireComponentInParent<ITargetSearcher>();
        }

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            _fireCoroutine = StartCoroutine(FireQueue(target, projectileParams, hitCallback));
        }

        private IEnumerator FireQueue(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var initialTarget = target;
            var singleShotParams = BuildSingleShotParams(projectileParams);
            var targets = MultiTargetRangedWeapon.FindAdditionalTargets(
                _targetSearcher,
                initialTarget,
                projectileParams.Count,
                projectileParams.DamageRadius);
            
            for (int i = 0; i < projectileParams.Count; i++)
            {
                if (targets.Count != 0)
                {
                    var nextTarget = targets.First();
                    targets.Remove(nextTarget);
                    base.Fire(nextTarget, singleShotParams, hitCallback);
                    continue;
                }
                
                yield return WaitForFixedSubInterval();
                base.Fire(initialTarget, singleShotParams, hitCallback);
            }
        }

        private IProjectileParams BuildSingleShotParams(IProjectileParams projectileParams)
        {
            return new ProjectileParams
            {
                Count = 1,
                Speed = projectileParams.Speed,
                DamageRadius = projectileParams.DamageRadius,
                AttackDistance = projectileParams.AttackDistance
            };
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
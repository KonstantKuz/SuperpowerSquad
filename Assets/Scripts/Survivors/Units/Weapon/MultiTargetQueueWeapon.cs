using System;
using System.Collections;
using System.Linq;
using Feofun.Extension;
using ModestTree;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using Survivors.Util;
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
            var singleShotParams = BuildSingleShotParams(projectileParams);
            var targets = MultiTargetRangedWeapon.FindAdditionalTargets(
                _targetSearcher,
                target,
                projectileParams.Count,
                projectileParams.DamageRadius)
                .Except(target).ToList();
            
            targets.ForEach(it => base.Fire(it, singleShotParams, hitCallback));
            
            for (int i = 0; i < projectileParams.Count - targets.Count; i++)
            {
                base.Fire(target, singleShotParams, hitCallback);
                yield return CoroutineUtil.WaitForSecondsFixedTime(_subInterval);
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
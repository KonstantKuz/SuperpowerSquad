using System;
using System.Collections.Generic;
using System.Linq;
using LegionMaster.Extension;
using Survivors.Units.Player.Attack;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Weapon
{
    public class MultiTargetRangedWeapon: RangedWeapon
    {
        [SerializeField] private NearestTargetSearcher _targetSearcher;
        
        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var targets = new List<ITarget> { target };
            FindAdditionalTargets(targets, projectileParams.Count, projectileParams.DamageRadius);
            for (int i = 0; i < projectileParams.Count; i++)
            {
                base.Fire(targets[i], new ProjectileParams
                {
                    Count = 1,
                    DamageRadius = projectileParams.DamageRadius, 
                    Speed = projectileParams.Speed
                }, hitCallback);
            }
        }

        private void FindAdditionalTargets(List<ITarget> targets, int targetCount, float distanceBetweenTargets)
        {
            var allTargets = _targetSearcher.GetAllOrderedByDistance().ToList();
            targets.ForEach(it => allTargets.Remove(it));
            
            TryAddDistinctTargets(targets, targetCount, distanceBetweenTargets, allTargets);
            TryAddRandomTargets(targets, targetCount, allTargets);
        }

        private static void TryAddRandomTargets(List<ITarget> targets, int targetCount, List<ITarget> allTargets)
        {
            while (targets.Count < targetCount && allTargets.Count > 0)
            {
                var newTarget = allTargets.Random();
                if (newTarget == null) return;
                allTargets.Remove(newTarget);
                targets.Add(newTarget);
            }
        }

        private static void TryAddDistinctTargets(ICollection<ITarget> targets, int targetCount, float distanceBetweenTargets, ICollection<ITarget> allTargets)
        {
            while (targets.Count < targetCount && allTargets.Count > 0)
            {
                var newTarget = allTargets
                    .FirstOrDefault(it =>
                        Vector3.Distance(targets.Last().Center.position, it.Center.position) >= distanceBetweenTargets);
                if (newTarget == null) return;
                allTargets.Remove(newTarget);
                targets.Add(newTarget);
            }
        }
    }
}
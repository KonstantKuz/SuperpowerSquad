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
            var targets = FindAdditionalTargets(target, projectileParams.Count, projectileParams.DamageRadius);
            foreach (var t in targets)
            {
                base.Fire(t, 
                    new ProjectileParams
                    {
                        Count = 1,
                        DamageRadius = projectileParams.DamageRadius, 
                        Speed = projectileParams.Speed
                    }, 
                    hitCallback);
            }
        }

        private List<ITarget> FindAdditionalTargets(ITarget initialTarget, int targetCount, float minDistanceBetweenTargets)
        {
            var selectedTargets = new List<ITarget> { initialTarget };            
            var possibleTargets = _targetSearcher
                .GetAllOrderedByDistance()
                .Except(selectedTargets)                
                .ToList();
            
            SelectDistinctTargets(selectedTargets, targetCount - selectedTargets.Count, minDistanceBetweenTargets, possibleTargets);
            SelectRandomTargets(selectedTargets, targetCount - selectedTargets.Count, possibleTargets);
            return selectedTargets;
        }

        private static void SelectRandomTargets(ICollection<ITarget> selectedTargets, int countToSelect, List<ITarget> possibleTargets)
        {
            for (int i = 0; i < countToSelect; i++)
            {
                if (possibleTargets.Count == 0) return;
                var newTarget = possibleTargets.Random();
                if (newTarget == null) return;
                
                possibleTargets.Remove(newTarget);
                selectedTargets.Add(newTarget);
            }
        }

        private static void SelectDistinctTargets(ICollection<ITarget> selectedTargets, int countToSelect, float distanceBetweenTargets, ICollection<ITarget> possibleTargets)
        {
            for (int i = 0; i < countToSelect; i++) {
                var newTarget = possibleTargets
                    .FirstOrDefault(it =>
                        Vector3.Distance(selectedTargets.Last().Center.position, it.Center.position) >= distanceBetweenTargets);
                if (newTarget == null) return;
                
                possibleTargets.Remove(newTarget);
                selectedTargets.Add(newTarget);
            }
        }
    }
}
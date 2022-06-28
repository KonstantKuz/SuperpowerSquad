using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Units.Target
{
    [PublicAPI]
    public class TargetService
    {
        private readonly Dictionary<UnitType, HashSet<ITarget>> _targets = new Dictionary<UnitType, HashSet<ITarget>>();
        public void Add(ITarget target)
        {
            if (!_targets.ContainsKey(target.UnitType)) {
                _targets[target.UnitType] = new HashSet<ITarget>();
            }
            _targets[target.UnitType].Add(target);
        }

        public void Remove(ITarget target)
        {
            _targets[target.UnitType].Remove(target);
        }
        
        public IEnumerable<ITarget> AllTargetsOfType(UnitType unitType) =>
                _targets.ContainsKey(unitType) ? _targets[unitType] : Enumerable.Empty<ITarget>();

        public IEnumerable<ITarget> GetTargetsInRadius(Vector3 from, UnitType targetType, float radius)
        {
            foreach (var target in AllTargetsOfType(targetType))
            {
                var distance = Vector3.Distance(from, target.Root.position);
                if(distance > radius)
                    continue;
                yield return target;
            }
        }
        
        [CanBeNull]
        public ITarget FindClosestTargetOfType(UnitType unitType, Vector3 pos)
        {
            return AllTargetsOfType(unitType).OrderBy(it => Vector3.Distance(it.Root.position, pos)).FirstOrDefault();
        }
    }
}
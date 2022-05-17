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
            target.OnTargetInvalid += Remove;
        }

        public void Remove(ITarget target)
        {
            _targets[target.UnitType].Remove(target);
        }

        public IEnumerable<ITarget> AllTargetsOfType(UnitType unitType) =>
                _targets.ContainsKey(unitType) ? _targets[unitType] : Enumerable.Empty<ITarget>();

        [CanBeNull]
        public ITarget FindClosestTargetOfType(UnitType unitType, Vector3 pos)
        {
            return AllTargetsOfType(unitType).OrderBy(it => Vector3.Distance(it.Root.position, pos)).FirstOrDefault();
        }

     
    }
}
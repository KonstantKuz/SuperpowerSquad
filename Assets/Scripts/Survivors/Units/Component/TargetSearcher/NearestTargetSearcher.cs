using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using Survivors.Units.Model;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    [RequireComponent(typeof(ITarget))]
    public class NearestTargetSearcher : MonoBehaviour, IInitializable<IUnit>, ITargetSearcher
    {
        [Inject]
        private TargetService _targetService;

        private IAttackModel _attackModel;    
        private ITarget _selfTarget;
        private UnitType _targetType;

        private float SearchDistance => _attackModel.TargetSearchRadius;

        public void Init(IUnit unit)
        {
            _attackModel = unit.Model.AttackModel;
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetType = _selfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var targets = _targetService.AllTargetsOfType(_targetType);
            return Find(targets, _selfTarget.Root.position, SearchDistance);
        }

        [CanBeNull]
        public static ITarget Find(IEnumerable<ITarget> targets, Vector3 from, float searchDistance)
        {
            ITarget result = null;
            var minDistance = Mathf.Infinity;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var dist = Vector3.Distance(from, target.Root.position);
                if (dist >= minDistance || dist > searchDistance) continue;
                minDistance = dist;
                result = target;
            }
            
            return result;
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            return _targetService.AllTargetsOfType(_targetType)
                .Where(IsDistanceReached)
                .OrderBy(it => Vector3.Distance(it.Root.position, _selfTarget.Root.position));
        }

        private bool IsDistanceReached(ITarget target) => 
            Vector3.Distance(target.Root.position, _selfTarget.Root.position) <= SearchDistance;
    }
}
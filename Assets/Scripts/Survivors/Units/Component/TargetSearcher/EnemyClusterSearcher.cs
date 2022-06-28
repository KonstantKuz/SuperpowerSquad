using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    public class EnemyClusterSearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
    {
        private const float MAX_DISTANCE_IN_CLUSTER = 1.5f;       
        private const float UNIT_DISTANCE_OUTSIDE_GROUP = 1000;
        private IUnit _owner;
        private UnitType _targetType;

        [Inject]
        private TargetService _targetService;
        private float SearchDistance => _owner.Model.AttackModel.TargetSearchRadius;

        public void Init(IUnit owner)
        {
            _owner = owner;
            _targetType = _owner.SelfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var targets = GetTargetsInRadius().ToList();
            
            ITarget result = null;
            float minDistanceWeigh = float.MaxValue;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var weight = GetWeightBySumDistance(target, targets);
                if (weight > minDistanceWeigh) continue;
                minDistanceWeigh = weight;
                result = target;
            }
            return result;
        }
        
        private float GetWeightBySumDistance(ITarget researchedTarget, IEnumerable<ITarget> targets)
        {
            float distanceWight = 0;
            foreach (var target in targets) {
                var distance = (researchedTarget.Root.position - target.Root.position).magnitude; 
                distance = distance > MAX_DISTANCE_IN_CLUSTER ? UNIT_DISTANCE_OUTSIDE_GROUP : distance;
                distanceWight += distance;
            }
            return distanceWight;
        }
        
        private IEnumerable<ITarget> GetTargetsInRadius()
        {
            return _targetService.GetTargetsInRadius(_owner.SelfTarget.Root.position, _targetType, SearchDistance);
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            throw new NotImplementedException();
        }
    }
}
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
            return targets.OrderBy(it => GetWeightBySumDistance(it, targets)).FirstOrDefault();
        }
        
        private float GetWeightBySumDistance(ITarget target, IEnumerable<ITarget> targets)
        {
            return targets.Sum(it => (it.Root.position - target.Root.position).magnitude);
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
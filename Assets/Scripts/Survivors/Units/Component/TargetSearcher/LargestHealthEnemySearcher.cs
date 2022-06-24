using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using ModestTree;
using Survivors.Squad.Component;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    public class LargestHealthEnemySearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
    {
        private IUnit _owner;
        private UnitType _targetType;

        [Inject] private TargetService _targetService;

        private float SearchDistance => _owner.Model.AttackModel.TargetSearchRadius;
        
        public void Init(IUnit owner)
        {
            _owner = owner;
            _targetType = _owner.SelfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var targets = GetAllAtSearchDistance().ToArray();
            return FindWithLargestHealth(targets);
        }

        public ITarget FindExcept(IEnumerable<ITarget> exceptTargets)
        {
            var targets = GetAllAtSearchDistance().Except(exceptTargets);
            return FindWithLargestHealth(targets);
        }
        
        private ITarget FindWithLargestHealth(IEnumerable<ITarget> targets)
        {
            ITarget result = null;
            var maxHealth = 0f;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var health = target.Root.parent.GetComponent<Health.Health>();
                if(health == null) continue;
                if (health.CurrentValue.Value <= maxHealth) continue;
                maxHealth = health.CurrentValue.Value;
                result = target;
            }

            return result;
        }

        private IEnumerable<ITarget> GetAllAtSearchDistance()
        {
            return _targetService.AllTargetsOfType(_targetType).Where(IsAtSearchDistance);
        }

        private bool IsAtSearchDistance(ITarget target)
        {
            return Vector3.Distance(target.Root.position, _owner.SelfTarget.Root.position) <= SearchDistance;
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            throw new NotImplementedException();
        }
    }
}
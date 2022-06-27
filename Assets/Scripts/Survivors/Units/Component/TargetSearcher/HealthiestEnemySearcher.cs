using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    public class HealthiestEnemySearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
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
            return FindHealthiestTarget(GetTargetsInRadius());
        }
        
        public IEnumerable<ITarget> FindHealthiestTargets(int count)
        {
            var allTargets = GetTargetsInRadius().ToList();
            var targetsToReturn = new List<ITarget>( );
            for (int i = 0; i < count; i++)
            {
                if (allTargets.Count == 0)
                {
                    break;
                }
                var healthiestTarget = FindHealthiestTarget(allTargets);
                allTargets.Remove(healthiestTarget);
                targetsToReturn.Add(healthiestTarget);
            }
            return targetsToReturn;
        }
        
        [CanBeNull]
        private ITarget FindHealthiestTarget(IEnumerable<ITarget> targets)
        {
            ITarget result = null;
            var maxHealth = 0f;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var health = target.Root.parent.GetComponent<Health.Health>();
                if (health == null)
                {
                    Debug.LogWarning("One of the target has no Health component or its Root not parented to the object with Health component.");
                    continue;
                }
                if (health.CurrentValue.Value <= maxHealth) continue;
                maxHealth = health.CurrentValue.Value;
                result = target;
            }

            return result;
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
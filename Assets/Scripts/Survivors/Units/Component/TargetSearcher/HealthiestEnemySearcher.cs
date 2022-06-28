using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Units.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    public class HealthiestEnemySearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
    {
        private IUnit _owner;
        private UnitType _targetType;

        [Inject] private UnitService _unitService;
        
        private float SearchDistance => _owner.Model.AttackModel.TargetSearchRadius;
        
        public void Init(IUnit owner)
        {
            _owner = owner;
            _targetType = _owner.SelfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var healthiestUnit = FindHealthiestUnit(GetUnitsInRadius());
            return healthiestUnit == null ? null : healthiestUnit.SelfTarget;
        }
        
        public IEnumerable<Unit> FindHealthiestUnits(int count)
        {
            var unitsInRadius = GetUnitsInRadius().ToList();
            var targetsToReturn = new List<Unit>();
            for (int i = 0; i < count; i++)
            {
                if (unitsInRadius.Count == 0) { break; }

                var healthiestUnit = FindHealthiestUnit(unitsInRadius);
                if(healthiestUnit == null) { continue; }
                
                unitsInRadius.Remove(healthiestUnit);
                targetsToReturn.Add(healthiestUnit);
            }
            return targetsToReturn;
        }
        
        [CanBeNull]
        private Unit FindHealthiestUnit(IEnumerable<Unit> units)
        {
            Unit result = null;
            var maxHealth = 0f;
            foreach (var unit in units)
            {
                if (!unit.SelfTarget.IsAlive) continue;
                var health = unit.Health;
                if (health == null)
                {
                    Debug.LogWarning("One of the target has no Health component or its Root not parented to the object with Health component.");
                    continue;
                }
                if (health.CurrentValue.Value <= maxHealth) continue;
                maxHealth = health.CurrentValue.Value;
                result = unit;
            }

            return result;
        }

        private IEnumerable<Unit> GetUnitsInRadius()
        {
            return _unitService.GetUnitsInRadius(_owner.SelfTarget.Root.position, _targetType, SearchDistance);
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            throw new NotImplementedException();
        }
    }
}
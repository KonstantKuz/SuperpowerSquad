﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.Units.Model;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITarget))]
    public class NearestTargetSearcher : MonoBehaviour, IInitializable<IUnit>, ITargetSearcher
    {
        [Inject]
        private TargetService _targetService;

        private IAttackModel _attackModel;
        private UnitType _targetType;

        private float SearchDistance => _attackModel.TargetSearchRadius;

        public void Init(IUnit unit)
        {
            _attackModel = unit.Model.AttackModel;
            _targetType = GetComponent<ITarget>().UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            ITarget rez = null;
            var minDistance = Mathf.Infinity;
            var pos = transform.position;
            foreach (var target in _targetService.AllTargetsOfType(_targetType))
            {
                var dist = Vector3.Distance(pos, target.Root.position);
                if (dist >= minDistance || dist > SearchDistance) continue;
                minDistance = dist;
                rez = target;
            }
            
            return rez;
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            return _targetService.AllTargetsOfType(_targetType)
                .Where(IsDistanceReached)
                .OrderBy(it => Vector3.Distance(it.Root.position, transform.position));
        }

        private bool IsDistanceReached(ITarget target) => 
            Vector3.Distance(target.Root.position, transform.position) <= SearchDistance;
    }
}
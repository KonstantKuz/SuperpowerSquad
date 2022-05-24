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
    public class NearestTargetSearcher : MonoBehaviour, IUnitInitializable, ITargetSearcher
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
            return GetAllOrderedByDistance().FirstOrDefault();
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
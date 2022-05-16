using System.Linq;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITarget))]
    public class AngularTargetSearcher : MonoBehaviour, IUnitInitialization, ITargetSearcher
    {
        [Inject]
        private TargetService _targetService;
        
        private AttackModel _attackModel;
        private UnitType _targetType;
        
        private float Distance => _attackModel.AttackDistance;    
        private float Angle => _attackModel.AttackAngle;
        
        public void Init(PlayerUnit playerUnit)
        {
            _attackModel = playerUnit.Model.AttackModel;
            _targetType = GetComponent<ITarget>().UnitType.GetTargetUnitType();
        }
        [CanBeNull]
        public ITarget Find()
        {
            return _targetService.AllTargetsOfType(_targetType)
                                 .Where(it => IsDistanceReached(it) && IsAngleOfViewReached(it))
                                 .OrderBy(it => Vector3.Distance(it.Root.position, transform.position))
                                 .FirstOrDefault();
        }

        private bool IsDistanceReached(ITarget target) => Vector3.Distance(target.Root.position, transform.position) <= Distance;
        private bool IsAngleOfViewReached(ITarget target)
        {
            var direction = target.Root.position - transform.position;
            return Vector2.Angle(transform.forward.ToVector2XZ(), direction.ToVector2XZ()) <= Angle * 0.5f;
        }
    }
}
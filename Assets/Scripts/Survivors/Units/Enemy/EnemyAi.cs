using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour, IUnitInitializable, IUpdatableUnitComponent
    {
        [SerializeField] private float _squadAdditionalRadius = 5f;
        [SerializeField] private float _targetResetDistance = 2f;
        
        private NavMeshAgent _agent;
        private ITarget _target;
        private ITargetSearcher _targetSearcher;

        [Inject] private World _world;

        private float DistanceToSquad =>
            Vector3.Distance(transform.position, _world.Squad.Destination.transform.position);
        private bool IsSquadNearby => DistanceToSquad <= _world.Squad.SquadRadius + _squadAdditionalRadius;
        private float DistanceToTarget => CurrentTarget == null ?
            Mathf.Infinity : Vector3.Distance(transform.position, CurrentTarget.Root.position);
        private bool IsTargetNearby => DistanceToTarget <= _targetResetDistance;

        public NavMeshAgent NavMeshAgent => _agent;
        
        [CanBeNull] 
        public ITarget CurrentTarget
        {
            get => _target;
            private set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.OnTargetInvalid -= ClearTarget;
                }
                _target = value;
                if (_target != null)
                {
                    _target.OnTargetInvalid += ClearTarget;
                }
            }
        }

        public void Init(IUnit unit)
        {
            var model = (EnemyUnitModel) unit.Model;
            _agent.speed = model.MoveSpeed;
        }

        private void Awake()
        {
            _agent = gameObject.RequireComponent<NavMeshAgent>();
            _targetSearcher = gameObject.RequireComponent<ITargetSearcher>();
        }

        public void OnTick()
        {
            if (IsRequiredAnyTarget() || IsRequiredNearestTarget())
            {
                FindTarget();
            }

            if (CurrentTarget == null)
            {
                _agent.isStopped = true;
                return;
            }

            _agent.destination = CurrentTarget.Root.position;
            _agent.isStopped = false;
        }

        private bool IsRequiredAnyTarget()
        {
            return !IsSquadNearby && !IsTargetNearby;
        }

        private bool IsRequiredNearestTarget()
        {
            return IsSquadNearby && !IsTargetNearby;
        }
        
        private void FindTarget()
        {
            CurrentTarget = _targetSearcher.Find();
        }

        private void ClearTarget()
        {
            CurrentTarget = null;
        }
    }
}
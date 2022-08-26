using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IUnitActiveStateReceiver
    {
        public static int Count;
        private const float ACCURATE_FOLLOW_DISTANCE = 1f;
        
        [SerializeField] private float _targetSelectionDistance = 10f;
        [SerializeField] private float _agentRadiusAfar;
        [SerializeField] private float _agentRadiusNear;
        [SerializeField] private float _agentDistanceAfar;
        [SerializeField] private float _agentDistanceNear;

        private ITarget _selfTarget;
        private CapsuleCollider _collider;
        private NavMeshAgent _agent;
        private ITarget _target;
        private ITargetSearcher _targetSearcher;
        private float _initialAgentRadius;

        [Inject] private World _world;

        private Vector3 SquadPosition => _world.Squad.Destination.transform.position;
        private float AgentRadiusAfar => _agentRadiusAfar / Scale;   
        private float AgentRadiusNear => _agentRadiusNear / Scale;
        private float Scale => transform.localScale.x;
        private float SelfRadius => _collider.radius * Scale;
        private float DistanceToSquad => Vector3.Distance(_selfTarget.Root.position, SquadPosition) - SelfRadius;
        public float DistanceToTarget => CurrentTarget == null ? float.MaxValue : Vector3.Distance(_selfTarget.Root.position, CurrentTarget.Root.position) - SelfRadius;

        public NavMeshAgent NavMeshAgent => _agent;
        
        private bool Active { get; set; } = true;

        private bool Stopped {
            get => _agent.isStopped;
            set
            {
                if (!_agent.enabled) {
                    return;
                }
                if (!_agent.isOnNavMesh) {
                    return;
                }
                if (_agent.isStopped == value) {
                    return;
                }
                _agent.isStopped = value;
            }

        }

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
            
            name = gameObject.name + $"-{Count}";
            Count++;
            _agent = gameObject.RequireComponent<NavMeshAgent>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetSearcher = gameObject.RequireComponent<ITargetSearcher>();
            _collider = gameObject.RequireComponent<CapsuleCollider>();
            _initialAgentRadius = _agent.radius;
        }

        public void OnTick()
        {
            if (!Active || _world.Squad == null) return;
            UpdateAgentRadius();
            UpdateDestination();
        }

        private void UpdateDestination()
        {
            if (DistanceToSquad > _targetSelectionDistance) {
                MoveTo(SquadPosition);
                return;
            }
            CurrentTarget = FindTarget();
            
            if (CurrentTarget == null) {
                Stopped = true;
                return;
            }
            MoveTo(CurrentTarget.Root.position);
        }

        private void MoveTo(Vector3 destination)
        {
            Stopped = false;
            if (Vector3.Distance(transform.position, destination) > ACCURATE_FOLLOW_DISTANCE) {
                _agent.destination = transform.position + (destination - transform.position).normalized;
            }
            else {
                _agent.destination = destination;
            }
        }

        private void UpdateAgentRadius()
        {
            _agent.radius = _initialAgentRadius + Mathf.Lerp(AgentRadiusNear, AgentRadiusAfar, 
                                                             (DistanceToSquad - _agentDistanceNear) / (_agentDistanceAfar - _agentDistanceNear));
        }
        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();
        
        private void ClearTarget()
        {
            CurrentTarget = null;
        }

        public void OnActiveStateChanged(bool active)
        {
            Active = active;
            Stopped = !active;
        }
    }
}
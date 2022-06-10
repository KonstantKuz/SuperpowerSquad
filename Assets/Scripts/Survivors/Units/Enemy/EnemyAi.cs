using Feofun.Components;
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
    public class EnemyAi : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IUnitDeactivateEventReceiver
    {
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
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetSearcher = gameObject.RequireComponent<ITargetSearcher>();
            _collider = gameObject.RequireComponent<CapsuleCollider>();
            _initialAgentRadius = _agent.radius;
        }

        public void OnTick()
        {
            if (_world.Squad == null) return;
            
            UpdateAgentRadius();
            
            if (DistanceToSquad > _targetSelectionDistance) 
            {
                SetDestination(SquadPosition);
                return;
            }
            
            FindTarget();

            if (CurrentTarget == null)
            {
                _agent.isStopped = true;
                return;
            }

            SetDestination(CurrentTarget.Root.position);
            _agent.isStopped = false;
        }

        private void SetDestination(Vector3 destination)
        {
            if (Vector3.Distance(transform.position, destination) > 1)
            {
                _agent.destination = transform.position + (destination - transform.position).normalized;
            }
            else
            {
                _agent.destination = destination;
            }
        }

        private void UpdateAgentRadius()
        {
            _agent.radius = _initialAgentRadius + Mathf.Lerp(AgentRadiusNear, AgentRadiusAfar, 
                                                             (DistanceToSquad - _agentDistanceNear) / (_agentDistanceAfar - _agentDistanceNear));
        }

        private void FindTarget()
        {
            CurrentTarget = _targetSearcher.Find();
        }

        private void ClearTarget()
        {
            CurrentTarget = null;
        }

        public void OnDeactivate()
        {
            _agent.isStopped = true;
        }
    }
}
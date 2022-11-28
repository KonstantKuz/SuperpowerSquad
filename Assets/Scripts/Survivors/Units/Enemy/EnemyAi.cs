using Feofun.Components;
using Feofun.Extension;
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
        [SerializeField] private float _targetSelectionDistance = 10f;

        private ITarget _selfTarget;
        private CapsuleCollider _collider;
        private IAimController _aimController;
        private EnemyTargetProvider _targetProvider;
        private EnemyMovement _movement;

        [Inject] private World _world;

        public ITarget CurrentTarget => _targetProvider.CurrentTarget;
        private Vector3 SquadPosition => _world.Squad.Position;
        public float Scale => transform.localScale.x;
        public float SelfRadius => _collider.radius * Scale;
        public float DistanceToSquad => Vector3.Distance(_selfTarget.Root.position, SquadPosition) - SelfRadius;
        public float DistanceToTarget => CurrentTarget == null ? float.MaxValue : Vector3.Distance(_selfTarget.Root.position, CurrentTarget.Root.position) - SelfRadius;

        public NavMeshAgent NavMeshAgent => _movement.Agent;
        
        public bool Active { get; private set; } = true;
        
        public void Init(IUnit unit)
        {
            NavMeshAgent.enabled = true;
            var model = (EnemyUnitModel) unit.Model;
            NavMeshAgent.speed = model.MoveSpeed;
        }
        
        private void Awake()
        {
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _collider = gameObject.RequireComponent<CapsuleCollider>();
            _aimController = gameObject.RequireComponent<IAimController>();
            _movement = gameObject.RequireComponent<EnemyMovement>();
            _targetProvider = new EnemyTargetProvider(gameObject.RequireComponent<ITargetSearcher>());
        }

        public void OnActiveStateChanged(bool active)
        {
            Active = active;
            _movement.IsStopped = !active;
        }

        public void OnTick()
        {
            if (!Active || _world.Squad == null) return;
            UpdateDestination();
            if (_aimController.IsNeedAim) {
                AimAtTarget();
            }
            _movement.UpdateAnimation();
        }
        
        private void UpdateDestination()
        {
            _movement.IsStopped = false;
            if (DistanceToSquad > _targetSelectionDistance) {
                _movement.MoveTo(SquadPosition);
                return;
            }
            if (CurrentTarget == null) {
                _movement.IsStopped = true;
                return;
            }
            _movement.MoveTo(CurrentTarget.Root.position);
        }

        private void AimAtTarget()
        {
            _movement.IsStopped = true;
            _movement.LookAt(CurrentTarget.Root.position);
        }
    }
}
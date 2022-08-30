﻿using Feofun.Components;
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
        [SerializeField] private float _targetSelectionDistance = 10f;
        [SerializeField] private float _agentRadiusAfar;
        [SerializeField] private float _agentRadiusNear;
        [SerializeField] private float _agentDistanceAfar;
        [SerializeField] private float _agentDistanceNear;

        private ITarget _selfTarget;
        private CapsuleCollider _collider;
        private EnemyTargetProvider _targetProvider;
        private EnemyMovement _movement;
        private AgentRadiusHandler _agentRadiusHandler;

        [Inject] private World _world;

        public ITarget CurrentTarget => _targetProvider.CurrentTarget;
        private Vector3 SquadPosition => _world.Squad.Destination.transform.position;
        private float Scale => transform.localScale.x;
        private float SelfRadius => _collider.radius * Scale;
        private float DistanceToSquad => Vector3.Distance(_selfTarget.Root.position, SquadPosition) - SelfRadius;
        public float DistanceToTarget => CurrentTarget == null ? float.MaxValue : Vector3.Distance(_selfTarget.Root.position, CurrentTarget.Root.position) - SelfRadius;

        public NavMeshAgent NavMeshAgent => _movement.Agent;
        
        public bool Active { get; private set; } = true;
        
        public void Init(IUnit unit)
        {
            var model = (EnemyUnitModel) unit.Model;
            NavMeshAgent.speed = model.MoveSpeed;
        }
        private void Awake()
        {
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _collider = gameObject.RequireComponent<CapsuleCollider>();
            var agent = gameObject.RequireComponent<NavMeshAgent>();
            _targetProvider = new EnemyTargetProvider(gameObject.RequireComponent<ITargetSearcher>());
            _movement = new EnemyMovement(_selfTarget, agent, gameObject.RequireComponentInChildren<Animator>());
            _agentRadiusHandler = new AgentRadiusHandler(agent, _agentRadiusAfar, _agentRadiusNear,
                _agentDistanceNear, _agentDistanceAfar);
        }

        public void AimAtTarget()
        {
            _movement.IsStopped = true;
            _movement.LookAt(CurrentTarget.Root.position);
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
                _movement.MoveTo(SquadPosition);
                return;
            }
            if (CurrentTarget == null) {
                _movement.IsStopped = true;
                return;
            }
            _movement.MoveTo(CurrentTarget.Root.position);
        }

        private void UpdateAgentRadius()
        {
            _agentRadiusHandler.UpdateRadius(DistanceToSquad, Scale);
        }

        public void OnActiveStateChanged(bool active)
        {
            Active = active;
            _movement.IsStopped = !active;
        }
    }
}
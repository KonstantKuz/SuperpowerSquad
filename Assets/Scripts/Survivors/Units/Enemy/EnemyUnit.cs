using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Model;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(UnitWithHealth))]
    [RequireComponent(typeof(DestroyDeath))]
    public class EnemyUnit : Unit
    {
        private NavMeshAgent _agent;
        private ITarget _target;

        [Inject]
        private TargetService _targetService;

        public NavMeshAgent NavMeshAgent => _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _target ??= _targetService.FindClosestTargetOfType(UnitType.PLAYER, transform.position);

            if (_target != null) {
                _agent.destination = _target.Root.position;
                _agent.isStopped = false;
            } else {
                _agent.isStopped = true;
            }
        }
    }
}
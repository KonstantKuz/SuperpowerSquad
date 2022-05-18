using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour
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

            if (_target is {IsAlive: true}) {
                _agent.destination = _target.Root.position;
                _agent.isStopped = false;
            } else {
                _agent.isStopped = true;
            }
        }
    }
}
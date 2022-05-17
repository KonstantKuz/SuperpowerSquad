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
    [RequireComponent(typeof(EnemyDeath))]
    [RequireComponent(typeof(UnitTarget))]
    public class EnemyUnit : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private UnitWithHealth _health;
        private EnemyDeath _enemyDeath;
        private ITarget _target;
        private UnitTarget _unitTarget;

        [Inject]
        private TargetService _targetService;

        public NavMeshAgent NavMeshAgent => _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<UnitWithHealth>();
            _enemyDeath = GetComponent<EnemyDeath>();
            _unitTarget = GetComponent<UnitTarget>();
        }

        public void Init(IUnitHealthModel healthModel)
        {
            _health.Init(healthModel, OnDeath);
        }

        private void OnDeath()
        {
            _targetService.Remove(GetComponent<ITarget>());
            _enemyDeath.Die();
            _unitTarget.OnDeath();
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
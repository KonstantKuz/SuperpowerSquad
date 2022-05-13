using System;
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
    public class EnemyAi : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private UnitWithHealth _health;
        private EnemyDeath _enemyDeath;
        private ITarget _target;

        [Inject] 
        private TargetService _targetService;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<UnitWithHealth>();
            _enemyDeath = GetComponent<EnemyDeath>();
        }

        public void Init(IUnitHealthModel healthModel)
        {
            _health.Init(healthModel);
            _health.OnDeath += Death;
        }

        private void Death()
        {
            _health.OnDeath -= Death;
            _enemyDeath.Death();
        }

        private void Update()
        {
            _target ??= _targetService.FindClosestTargetOfType(UnitType.PLAYER, transform.position);

            if (_target != null)
            {
                _agent.destination = _target.Root.position;
                _agent.isStopped = false;
            }
            else
            {
                _agent.isStopped = true;
            }
        }
    }
}
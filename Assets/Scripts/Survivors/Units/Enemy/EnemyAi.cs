using JetBrains.Annotations;
using Survivors.Extension;
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
        private NavMeshAgent _agent;
        private ITarget _target;
        private ITargetSearcher _targetSearcher;

        public NavMeshAgent NavMeshAgent => _agent;
        [CanBeNull] 
        public ITarget CurrentTarget => _target;

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
            if (_target == null)
            {
                _agent.isStopped = true;
                FindTarget();
                return;
            }

            _agent.destination = _target.Root.position;
            _agent.isStopped = false;
        }

        private void FindTarget()
        {
            _target = _targetSearcher.Find();
            if (_target == null)
            {
                return;
            }
            _target.OnTargetInvalid += ClearTarget;
        }

        private void ClearTarget()
        {
            _target.OnTargetInvalid -= ClearTarget;
            _target = null;
        }
    }
}
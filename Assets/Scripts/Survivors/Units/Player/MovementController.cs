using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");
        
        private NavMeshAgent _agent;
        private Animator _animator;

        private NavMeshAgent Agent => _agent ??= GetComponent<NavMeshAgent>();

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void MoveTo(Vector3 destination)
        {
            Agent.destination = destination;
            if (IsDestinationReached)
            {
                Stop();
                return;
            }
            Agent.isStopped = false;
            _animator.Play(_runHash);
        }

        private bool IsDestinationReached => _agent.remainingDistance < _agent.stoppingDistance;

        public void Stop()
        {
            Agent.isStopped = true;
            _animator.Play(_idleHash);
        }

        public void SetSpeed(float speed)
        {
            Agent.speed = speed;
        }

        private void Update()
        {
            if (!_agent.isStopped && IsDestinationReached)
            {
                Stop();
            }
        }
    }
}
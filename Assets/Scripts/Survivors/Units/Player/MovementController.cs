using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");
        
        private NavMeshAgent _agent;
        private Animator _animator;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.isStopped = false;
            _agent.destination = destination;
            _animator.Play(_runHash);
        }

        public void Stop()
        {
            _agent.isStopped = true;
            _animator.Play(_idleHash);
        }
    }
}
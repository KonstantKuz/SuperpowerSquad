using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.PlayerUnit
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");
        
        private NavMeshAgent _agent;
        private Animator _animator;

        [Inject] 
        private Joystick _joystick;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (_joystick.Direction.sqrMagnitude > 0)
            {
                _agent.isStopped = false;
                _agent.destination = transform.position + new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
                _animator.Play(_runHash);
            }
            else
            {
                _agent.isStopped = true;
                _animator.Play(_idleHash);
            }
        }
    }
}
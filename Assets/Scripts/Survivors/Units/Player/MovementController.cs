using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private const float MAX_ROTATE_ANIMATION_ANGLE = 90;
 

        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _horizontalMotionHash = Animator.StringToHash("HorizontalMotion");

        [SerializeField]
        private Transform _root;

        private NavMeshAgent _agent;
        private Animator _animator;

        private NavMeshAgent Agent => _agent ??= GetComponent<NavMeshAgent>();
        private bool IsDestinationReached => _agent.remainingDistance < _agent.stoppingDistance;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!Agent.isStopped && IsDestinationReached) {
                Stop();
            }
            UpdateAnimationRotation();
        }

        public void MoveTo(Vector3 destination)
        {
            Agent.destination = destination;
            if (IsDestinationReached) {
                Stop();
                return;
            }
            Agent.isStopped = false;
            _animator.Play(_runHash);
        }

        public void Stop()
        {
            Agent.isStopped = true;
            _animator.Play(_idleHash);
        }

        public void SetSpeed(float speed)
        {
            Agent.speed = speed;
        }

        private void UpdateAnimationRotation()
        {
            if (IsDestinationReached) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            DrawDebugRay(transform.forward, Color.red);
            DrawDebugRay(_root.forward, Color.yellow);
            var angle = Vector2.SignedAngle(transform.forward.ToVector2XZ(), _root.forward.ToVector2XZ());
            Debug.Log($"Angle:= {angle}");
            
            var animationOffsetValue = Mathf.Abs(angle / MAX_ROTATE_ANIMATION_ANGLE);
            
            if (angle >= -90 && angle <= 0) {
                _animator.SetFloat(_horizontalMotionHash, -animationOffsetValue);
                _animator.SetFloat(_verticalMotionHash, 1); 
                return;
       
            } 
            if (angle <= 90 && angle >= 0) {
                _animator.SetFloat(_horizontalMotionHash, animationOffsetValue);
                _animator.SetFloat(_verticalMotionHash, 1);
                return;

            }
            if (angle <= -90 && angle >= -180) {
                _animator.SetFloat(_horizontalMotionHash, -(2 - animationOffsetValue));
                _animator.SetFloat(_verticalMotionHash, -1);
                return;

            } 
            if (angle >= 90 && angle <= 180) {
                _animator.SetFloat(_horizontalMotionHash, 2 - animationOffsetValue);
                _animator.SetFloat(_verticalMotionHash, -1);
                return;

            }
        }

        private void DrawDebugRay(Vector3 rayDirection, Color color)
        {
            var debugRay = rayDirection * 10;
            Debug.DrawRay(transform.position, debugRay, color, .1f, false);
        }
        
    }
}
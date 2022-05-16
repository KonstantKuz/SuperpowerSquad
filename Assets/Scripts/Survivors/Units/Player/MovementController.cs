using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private const float MAX_ROTATE_ANIMATION_ANGLE = 90;
        private const float FORWARD_DIRECTION_ANIMATION_PARAM = 0.5f;

        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        private readonly int _rotateToSideHash = Animator.StringToHash("RotateToSide");

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

        public void PlayUnitRotateAnimation(float signedAngle)
        {
            _animator.SetFloat(_rotateToSideHash, CalculateRotateAnimationParam(signedAngle));
        }

        private float CalculateRotateAnimationParam(float signedAngle)
        {
            var animationOffsetValue = Mathf.Abs((signedAngle * FORWARD_DIRECTION_ANIMATION_PARAM) / MAX_ROTATE_ANIMATION_ANGLE);
            if (signedAngle < 0) {
                return Mathf.Max(0, FORWARD_DIRECTION_ANIMATION_PARAM - animationOffsetValue);
            }
            if (signedAngle > 0) {
                Mathf.Min(1, FORWARD_DIRECTION_ANIMATION_PARAM + animationOffsetValue);
            }
            return FORWARD_DIRECTION_ANIMATION_PARAM;
        }
    }
}
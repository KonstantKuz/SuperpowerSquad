using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private const float MAX_ROTATE_ANIMATION_ANGLE = 90;
        private const float FORWARD_DIRECTION_ANIMATION_PARAM = 0.5f;

        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _turnToSideHash = Animator.StringToHash("TurnToSide");

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
            if (_joystick.Direction.sqrMagnitude > 0) {
                _agent.isStopped = false;
                _agent.destination = transform.position + new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
                _animator.Play(_runHash);
            } else {
                _agent.isStopped = true;
                _animator.Play(_idleHash);
            }
        }

        public void PlayUnitRotateAnimation(float signedAngle)
        {
            _animator.SetFloat(_turnToSideHash, CalculateRotateAnimationParam(signedAngle));
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
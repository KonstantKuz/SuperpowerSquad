using System;
using JetBrains.Annotations;
using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour, IUnitDeathEventReceiver
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _horizontalMotionHash = Animator.StringToHash("HorizontalMotion");

        [SerializeField]
        private Transform _rotationRoot;
        [SerializeField]
        private float _rotationSpeed = 10;

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
            UpdateAnimationRotateValues();
        }

        public void Init(float speed)
        {
            Agent.speed = speed;
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

        public void RotateToTarget([CanBeNull] Transform target)
        {
            if (target != null) {
                RotateTo(target.position);
            } else {
                Rotate(Quaternion.LookRotation(transform.forward));
            }
        }
        
        public void OnDeath()
        {
            Stop();
        }
        
        private void Stop()
        {
            Agent.isStopped = true;
            _animator.Play(_idleHash);
        }
        private void RotateTo(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - _rotationRoot.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, _rotationRoot.up);
            Rotate(lookAt);
        }
        private void Rotate(Quaternion lookAt)
        {
            _rotationRoot.rotation = Quaternion.Lerp(_rotationRoot.rotation, lookAt, Time.deltaTime * _rotationSpeed);
        }
        private void UpdateAnimationRotateValues()
        {
            if (IsDestinationReached || Agent.isStopped) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            var signedAngle = GetRotateSignedAngle();
            _animator.SetFloat(_horizontalMotionHash, (float) Math.Sin(GetRadian(signedAngle)));
            _animator.SetFloat(_verticalMotionHash, (float) Math.Cos(GetRadian(signedAngle)));
        }
        private double GetRadian(float signedAngle) => Mathf.Deg2Rad * signedAngle;
        private float GetRotateSignedAngle() => Vector2.SignedAngle(transform.forward.ToVector2XZ(), _rotationRoot.forward.ToVector2XZ());
    }
}
using System;
using JetBrains.Annotations;
using Survivors.Extension;
using UniRx;
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
        [SerializeField] 
        private float _speedMultiplier;

        private NavMeshAgent _agent;
        private Animator _animator;
        private CompositeDisposable _disposable;
        private NavMeshAgent Agent => _agent ??= GetComponent<NavMeshAgent>();
        private bool IsDestinationReached => _agent.remainingDistance < _agent.stoppingDistance;
        private float _squadSpeed;

        public void Init(IReadOnlyReactiveProperty<float> speed)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            speed.Subscribe(value => _squadSpeed = value).AddTo(_disposable);
        }
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        private void Update()
        {
            if (!Agent.isStopped && IsDestinationReached) {
                Stop();
            }

            IncreaseSpeedWithDistanceToDestination();
            UpdateAnimationRotateValues();
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
            _disposable?.Dispose();
            _disposable = null;
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

        private void IncreaseSpeedWithDistanceToDestination()
        {
            if (_agent.isStopped)
            {
                _agent.speed = _squadSpeed;
                return;
            }

            _agent.speed = _squadSpeed * (1.0f + Mathf.Pow(Mathf.Max(0.0f,
                _agent.remainingDistance - _agent.stoppingDistance), 2) * _speedMultiplier);
        }
    }
}
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
        private float _rotationSpeed = 10;
        
        private Animator _animator;
        private CompositeDisposable _disposable;
        public bool HasTarget { get; private set; }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void PlayAnimation(bool isMoving)
        {
            _animator.Play(isMoving ? _runHash : _idleHash);
            UpdateAnimationRotateValues(isMoving);
        }

        public void RotateTo(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - transform.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
        }
        public void RotateToTarget([CanBeNull] Transform target)
        {
            if (target != null) {
                HasTarget = true;
                RotateTo(target.position);
            } else {
                HasTarget = false;
            }
        }
        
        public void OnDeath()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        private void UpdateAnimationRotateValues(bool isMoving)
        {
            if (!isMoving) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            var signedAngle = GetRotateSignedAngle();
            _animator.SetFloat(_horizontalMotionHash, (float) Math.Sin(GetRadian(signedAngle)));
            _animator.SetFloat(_verticalMotionHash, (float) Math.Cos(GetRadian(signedAngle)));
        }
        private double GetRadian(float signedAngle) => Mathf.Deg2Rad * signedAngle;
        private float GetRotateSignedAngle() => Vector2.SignedAngle(transform.forward.ToVector2XZ(), transform.forward.ToVector2XZ());
    }
}
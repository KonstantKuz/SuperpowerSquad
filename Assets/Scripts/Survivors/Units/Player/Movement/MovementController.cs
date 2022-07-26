using System;
using JetBrains.Annotations;
using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour, IUnitDeathEventReceiver, IUnitActiveStateReceiver
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _horizontalMotionHash = Animator.StringToHash("HorizontalMotion");

        [SerializeField]
        private float _rotationSpeed = 10;
        
        private Animator _animator;
        public bool HasTarget { get; private set; }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void UpdateAnimation(Vector3 moveDirection)
        {
            PlayAnimation(moveDirection.sqrMagnitude > 0);
            UpdateAnimationRotateValues(moveDirection);
            if (HasTarget) return;
            RotateTo(transform.position + moveDirection);
        }

        private void PlayAnimation(bool isMoving)
        {
            _animator.Play(isMoving ? _runHash : _idleHash);
        }

        private void RotateTo(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - transform.position).XZ().normalized;
            if (lookAtDirection == Vector3.zero) { return; }
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
        public void OnActiveStateChanged(bool active)
        {
            if (!active) {
                StopAnimation();
            }
        }
        public void OnDeath(DeathCause deathCause)
        {
            StopAnimation();
        }

        private void StopAnimation()
        {
            _animator.Play(_idleHash);
            _animator.SetFloat(_horizontalMotionHash, 0);
            _animator.SetFloat(_verticalMotionHash, 0);
        }

        private void UpdateAnimationRotateValues(Vector3 moveDirection)
        {
            if (moveDirection.sqrMagnitude <= 0) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            var signedAngle = GetRotateSignedAngle(moveDirection);
            _animator.SetFloat(_horizontalMotionHash, (float) Math.Sin(GetRadian(signedAngle)));
            _animator.SetFloat(_verticalMotionHash, (float) Math.Cos(GetRadian(signedAngle)));
        }
        private double GetRadian(float signedAngle) => Mathf.Deg2Rad * signedAngle;
        private float GetRotateSignedAngle(Vector3 moveDirection) => Vector2.SignedAngle(transform.forward.ToVector2XZ(), moveDirection.ToVector2XZ());
       
    }
}
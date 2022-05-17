﻿using System;
using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player.Movement
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
        private Transform _rotationRoot;

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

        private void Stop()
        {
            Agent.isStopped = true;
            _animator.Play(_idleHash);
        }

        public void SetSpeed(float speed)
        {
            Agent.speed = speed;
        }

        private void UpdateAnimationRotateValues()
        {
            if (IsDestinationReached) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            var signedAngle = GetRotateSignedAngle();
            _animator.SetFloat(_horizontalMotionHash, (float) Math.Sin(GetRadian(signedAngle)));
            _animator.SetFloat(_verticalMotionHash, (float) Math.Cos(GetRadian(signedAngle)));
        }
        private double GetRadian(float signedAngle) => (Math.PI / 180) * signedAngle;
        private float GetRotateSignedAngle() => Vector2.SignedAngle(transform.forward.ToVector2XZ(), _rotationRoot.forward.ToVector2XZ());
    }
}
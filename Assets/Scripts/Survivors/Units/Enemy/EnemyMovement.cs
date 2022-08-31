﻿using System;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private const float ACCURATE_FOLLOW_DISTANCE = 1f;

        private ITarget _selfTarget;
        private NavMeshAgent _agent;
        private Animator _animator;
        private AnimationWrapper _animationWrapper;

        private bool IsAgentValid => _agent.enabled && _agent.isOnNavMesh;
        public NavMeshAgent Agent => _agent;
        
        public bool IsStopped 
        {
            get => _agent.isStopped;
            set => SetIsStopped(value);
        }

        private void Awake()
        {
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _agent = gameObject.RequireComponent<NavMeshAgent>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _animationWrapper = new AnimationWrapper(_animator);
        }

        private void SetIsStopped(bool value)
        {
            if (!IsAgentValid) {
                return;
            }
            if (_agent.isStopped == value) {
                return;
            }
            _agent.isStopped = value;
        }

        public void UpdateAnimation()
        {
            if (IsStopped) {
                _animationWrapper.PlayIdle();
            } else {
                _animationWrapper.PlayMoveForward();
            }
        }
        
        public void MoveTo(Vector3 destination)
        {
            if (Vector3.Distance(_selfTarget.Root.position, destination) > ACCURATE_FOLLOW_DISTANCE) {
                SetDestination(_selfTarget.Root.position + (destination - _selfTarget.Root.position).normalized);
            } else {
                SetDestination(destination); 
            }
        }

        public void LookAt(Vector3 target)
        {
            var lookDirection = (target - _selfTarget.Root.position).XZ();
            var lookRotation = Quaternion.LookRotation(lookDirection);
            _agent.transform.rotation = lookRotation;
        }

        private void SetDestination(Vector3 destination)
        {
            if (!_agent.isOnNavMesh) {
                this.Logger().Warn("SetDestination can only be called on an active agent that has been placed on a NavMesh.");
                return;
            }
            _agent.destination = destination;
        }
    }
}
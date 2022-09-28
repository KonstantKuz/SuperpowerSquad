using System;
using DG.Tweening;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Component.DamageReaction
{
    [RequireComponent(typeof(Unit))]  
    [RequireComponent(typeof(NavMeshAgent))]

    public class ExplosionReaction : MonoBehaviour
    {
        [SerializeField] private float _jumpRotationAngle;
        [Range(0f,1f)]
        [SerializeField] private float _jumpRotationTimeRatio;
        
        private Unit _owner;

        private NavMeshAgent _agent;
        private Sequence _explosionJump;
        
        private void Awake()
        {
            _owner = GetComponent<Unit>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public static void TryExecuteOn(GameObject target, Vector3 explosionPosition, ExplosionReactionParams reactionParams)
        {
            if (!target.TryGetComponent(out ExplosionReaction explosionReaction)) { return; }
            
            reactionParams.ExplosionPosition = explosionPosition;
            explosionReaction.OnExplosionReact(reactionParams);
        }
        
        public void OnExplosionReact(ExplosionReactionParams reactionParams)
        {
            if (gameObject == null) { return; } 
            if (!_owner.IsActive) { return; }
            
            var move = CreateJumpMove(reactionParams, out var jumpPosition);
            var rotate = CreateJumpRotation(reactionParams);
            _explosionJump = DOTween.Sequence();
            _explosionJump.Append(move).Insert(0, rotate).Play();
            
            _owner.Lock();
            _agent.enabled = false;
            
            _explosionJump.onComplete = () => {
                CompleteExplosionJump(jumpPosition); 

            };
        }
        private void CompleteExplosionJump(Vector3 jumpPosition)
        {
            _agent.enabled = true;
            _agent.Warp(jumpPosition);
            _owner.UnLock();
        }

        private Tween CreateJumpMove(ExplosionReactionParams reactionParams, out Vector3 jumpPosition)
        {
            var jumpDirection = transform.position - reactionParams.ExplosionPosition; 
            jumpPosition = transform.position + reactionParams.JumpDistance * Vector3.ProjectOnPlane(jumpDirection, Vector3.up) /  jumpDirection.magnitude;
            return transform.DOJump(jumpPosition, reactionParams.JumpHeight, 1, reactionParams.JumpDuration);
        }

        private Sequence CreateJumpRotation(ExplosionReactionParams reactionParams)
        {
            transform.LookAt(reactionParams.ExplosionPosition.XZ());
            var targetRotation = transform.rotation * Quaternion.Euler(-_jumpRotationAngle, 0, 0);
            var rotate = transform.DORotateQuaternion(targetRotation, reactionParams.JumpDuration * _jumpRotationTimeRatio);
            var rotateBack = transform.DORotateQuaternion(Quaternion.Euler(Vector3.zero), reactionParams.JumpDuration * (1f - _jumpRotationTimeRatio));
            return DOTween.Sequence().Append(rotate).Append(rotateBack);
        }

        private void OnDisable()=> Dispose();

        private void Dispose()
        {
            _explosionJump?.Kill(true); 
        }
    }
}
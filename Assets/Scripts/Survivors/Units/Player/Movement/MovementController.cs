using System;
using JetBrains.Annotations;
using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Player.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed = 10;
        
        public bool HasTarget { get; private set; }

        public void UpdateAnimation(Vector3 moveDirection)
        {
            if (HasTarget) return;
            RotateTo(transform.position + moveDirection);
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
    }
}
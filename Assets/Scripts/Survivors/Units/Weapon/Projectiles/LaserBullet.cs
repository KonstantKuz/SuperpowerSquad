﻿using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LaserBullet : Bullet
    {
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private BoxCollider _collider;
        [SerializeField]
        private float _widthFactor = 1;
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            SetWidth(projectileParams.DamageRadius);
        }

        private void SetWidth(float width)
        {
            if (Mathf.Abs(width) < Mathf.Epsilon) {
                return;
            }
            width *= _widthFactor;
            _lineRenderer.startWidth = width;      
            _lineRenderer.endWidth = width;
            var colliderSize = _collider.size;
            _collider.size = new Vector3(width, colliderSize.y, colliderSize.z);
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (!CanDamageTarget(collider, TargetType, out var target)) {
                return;
            }
            TryHit(collider.gameObject, transform.position, -transform.forward);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            PlayVfx(hitPos, collisionNorm);
        }
    }
}
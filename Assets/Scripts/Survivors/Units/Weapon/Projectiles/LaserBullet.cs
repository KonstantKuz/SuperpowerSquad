using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LaserBullet : Bullet
    {
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
            transform.localScale = width * Vector3.one;
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            PlayVfx(hitPos, collisionNorm);
        }
    }
}
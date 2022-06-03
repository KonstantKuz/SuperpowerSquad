using System;
using DG.Tweening;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Bomb : Projectile
    {
        [SerializeField]
        private Vector2 _explosionRadiusRange;
        [SerializeField]
        private Vector2 _explosionScaleRange;
        [SerializeField]
        private Vector2 _heightRange;
        [SerializeField]
        private Explosion _explosion;
        [SerializeField]
        private TrailRenderer _trail;

        [Inject]
        private WorldObjectFactory _objectFactory;
        
        public void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback, Vector3 targetPos)

        {
            base.Launch(target, projectileParams, hitCallback);
            var moveTime = GetFlightTime(targetPos);
            var maxHeight = GetMaxHeight(targetPos, projectileParams.AttackDistance);
            var jumpMove = transform.DOJump(targetPos, maxHeight, 1, moveTime);
            jumpMove.SetEase(Ease.Linear);
            jumpMove.onComplete = () => Explode(targetPos);
        }

        private float GetFlightTime(Vector3 targetPos)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return distanceToTarget / Params.Speed;
        }

        private float GetMaxHeight(Vector3 targetPos, float maxDistance)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return MathLib.Remap(distanceToTarget, 0, maxDistance, _heightRange.x, _heightRange.y);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            Explode(hitPos);
        }

        private void Explode(Vector3 pos)
        {
            var explosion = Explosion.Create(_objectFactory, _explosion, pos, Params.DamageRadius, TargetType, HitCallback);
            var scaleMultiplier = MathLib.Remap(Params.DamageRadius, _explosionRadiusRange.x, _explosionRadiusRange.y, _explosionScaleRange.x, _explosionScaleRange.y);
            explosion.transform.localScale *= scaleMultiplier;
            Destroy();
        }

        private void Destroy()
        {
            HitCallback = null;
            Destroy(gameObject);
            
            _trail.transform.SetParent(null);
            Destroy(_trail, _trail.time);
        }
    }
}
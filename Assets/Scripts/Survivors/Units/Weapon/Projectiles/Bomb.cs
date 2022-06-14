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
        [SerializeField] private float _heightMin;
        [SerializeField] private float _heightMax;

        [SerializeField] private float _explosionScaleMultiplier;
        [SerializeField] private Explosion _explosion;
        [SerializeField] private TrailRenderer _trail;

        private Tween _throwMove;
        
        [Inject]
        private WorldObjectFactory _objectFactory;
        
        public void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback, Vector3 targetPos)
        {
            base.Launch(target, projectileParams, hitCallback);
            var moveTime = GetFlightTime(targetPos);
            var maxHeight = GetMaxHeight(targetPos, projectileParams.AttackDistance);
            _throwMove = transform.DOJump(targetPos, maxHeight, 1, moveTime);
            _throwMove.SetEase(Ease.Linear);
            _throwMove.onComplete = () => Explode(targetPos);
        }

        private float GetFlightTime(Vector3 targetPos)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return distanceToTarget / Params.Speed;
        }

        private float GetMaxHeight(Vector3 targetPos, float maxDistance)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return MathLib.Remap(distanceToTarget, 0, maxDistance, _heightMin, _heightMax);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            Explode(hitPos);
        }

        private void Explode(Vector3 pos)
        {
            _throwMove.onComplete = null;
            _throwMove.Kill();
            
            var explosion = Explosion.Create(_objectFactory, _explosion, pos, Params.DamageRadius, TargetType, HitCallback);
            explosion.transform.localScale *= Params.DamageRadius * _explosionScaleMultiplier;
            Destroy();
        }

        private void Destroy()
        {
            HitCallback = null;
            _trail.transform.SetParent(null);
            Destroy(_trail.gameObject, _trail.time);
            Destroy(gameObject);
        }
    }
}
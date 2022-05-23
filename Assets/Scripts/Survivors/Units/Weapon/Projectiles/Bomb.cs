using System;
using DG.Tweening;
using Survivors.Location.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Bomb : Projectile
    {
        [SerializeField] private float _throwHeight;
        [SerializeField] private float _speed;
        [SerializeField] private Explosion _explosion;

        [Inject] private WorldObjectFactory _objectFactory;

        public override void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            var targetPos = target.Center.position;
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            var move = transform.DOJump(target.Center.position, _throwHeight, 1, distanceToTarget / _speed);
            move.SetEase(Ease.Linear);
            move.onComplete = () => Explode(targetPos);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            Explode(hitPos);
        }

        private void Explode(Vector3 pos)
        {
            Explosion.Create(_objectFactory, _explosion, pos, Params.DamageRadius, TargetType, HitCallback);
            Destroy();
        }

        private void Destroy()
        {
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}

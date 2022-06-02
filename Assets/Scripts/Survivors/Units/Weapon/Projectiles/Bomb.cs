using System;
using DG.Tweening;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Bomb : Projectile
    {
        [SerializeField]
        private Vector2 _heightRange;
        [SerializeField]
        private Explosion _explosion;
        [SerializeField]
        private GameObject _highlighterPrefab;

        [Inject]
        private WorldObjectFactory _objectFactory;
        private GameObject _highlighter;

        public void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback, Vector3 targetPos)
        {
            base.Launch(target, projectileParams, hitCallback);
            var moveTime = GetFlightTime(targetPos);
            var maxHeight = GetMaxHeight(targetPos, projectileParams.AttackDistance);
            CreateHighlighter(targetPos, projectileParams.DamageRadius);
            var jumpMove = transform.DOJump(targetPos, maxHeight, 1, moveTime);
            jumpMove.SetEase(Ease.Linear);
            jumpMove.onComplete = () => Explode(targetPos);
        }

        private void CreateHighlighter(Vector3 targetPos, float radius)
        {
            _highlighter = _objectFactory.CreateObject(_highlighterPrefab);
            _highlighter.transform.SetPositionAndRotation(targetPos.XZ(), Quaternion.identity);
            var scale = _highlighter.transform.localScale;
            _highlighter.transform.localScale = new Vector3(radius, scale.y, radius);
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
            Explosion.Create(_objectFactory, _explosion, pos, Params.DamageRadius, TargetType, HitCallback);
            Destroy();
        }

        private void Destroy()
        {
            if (_highlighter != null) {
                Destroy(_highlighter);
            }
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}
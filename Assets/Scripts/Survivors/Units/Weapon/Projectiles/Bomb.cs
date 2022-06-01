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
        [SerializeField] private Vector2 _heightRange;
        [SerializeField] private Explosion _explosion; 
        [SerializeField] private GameObject _highlighterPrefab;

        [Inject] private WorldObjectFactory _objectFactory;
        private GameObject _highlighter;

        public override void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            var targetPos = target.Center.position;
            var moveTime = GetFlightTime(targetPos);
            var maxHeight = GetMaxHeight(targetPos, projectileParams.AttackDistance);
            CreateHighlighter(targetPos);
            var jumpMove = transform.DOJump(targetPos, maxHeight, 1, moveTime);
            jumpMove.SetEase(Ease.Linear);
            jumpMove.onComplete = () => Explode(targetPos);
        }

        private void CreateHighlighter(Vector3 targetPos)
        {
            _highlighter = _objectFactory.CreateObject(_highlighterPrefab);
            _highlighter.transform.SetPositionAndRotation(targetPos.XZ(), Quaternion.identity);
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

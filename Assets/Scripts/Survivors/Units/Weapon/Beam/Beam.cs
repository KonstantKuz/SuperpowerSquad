using System;
using ModestTree;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Beam
{
    public class Beam : MonoBehaviour
    {
        [SerializeField] private float _maxLifeTime;
        [Range(0f, 1f)]
        [SerializeField] private float _ratioHitTime;
        
        protected ITarget _target;
        protected Action<GameObject> _hitCallback;
        
        private float _lifeTime;
        private bool _hit;
        private bool _initialized;
        private float HitTime => _maxLifeTime * _ratioHitTime;

        protected ITarget Target => _target; 
        
        public void Launch(ITarget target, Action<GameObject> hitCallback)
        {
            SetTarget(target);
            _hitCallback = hitCallback;
            _initialized = true;
        }
        protected virtual void SetTarget(ITarget target)
        {
            Assert.IsNotNull(target);
            if (_target != null) {
                ClearTarget();
            }
            _target = target;
            _target.OnTargetInvalid += ClearTarget;
        }
        protected virtual void Update()
        {
            if (!_initialized) {
                return;
            }
            UpdateLifeTime();
        }
        private void UpdateLifeTime()
        {
            _lifeTime += Time.deltaTime;
            if (_lifeTime >= HitTime && !_hit) {
                TryHit();
                ClearTarget();
            }
            if (_lifeTime >= _maxLifeTime) {
                Destroy();
            }
        }
        private void TryHit()
        {
            if (_hit) {
                return;
            }
            _hit = true;
            if (_target == null) {
                return;
            }
            Hit(_target);
        }
        
        protected virtual void Hit(ITarget target)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null) {
                Debug.LogWarning("Target is not a monobehaviour");
                return;
            }
            if (targetObj.gameObject.GetComponent<IDamageable>() == null) {
                return;
            }
            _hitCallback?.Invoke(targetObj.gameObject);
        }

        protected virtual void Destroy()
        {
            gameObject.SetActive(false);
            ClearTarget();
            _hitCallback = null;
            Destroy(gameObject);
        }
        
        protected virtual void ClearTarget()
        {
            if (_target != null) {
                _target.OnTargetInvalid -= ClearTarget;
            }
            _target = null;
        }

    }
}
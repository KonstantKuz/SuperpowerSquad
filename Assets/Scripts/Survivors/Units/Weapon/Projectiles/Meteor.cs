using System;
using Survivors.Location.Service;
using Survivors.Units.Component.Health;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField]
        private GameObject _hitVfx;
        [SerializeField]
        private float _explosionScaleMultiplier;
        [SerializeField] 
        private ExplosionReactionParams _explosionReactionParams;
    
        private Action<GameObject> _hitCallback;
        private UnitType _targetType;
        private IProjectileParams _params;
        private float _timeLeft;
        private float _speed;
        
        [Inject]
        private WorldObjectFactory _objectFactory;        

        public void Launch(UnitType targetType, IProjectileParams projectileParams, float lifeTime, float speed, Action<GameObject> hitCallback)
        {
            _hitCallback = hitCallback;
            _targetType = targetType;
            _timeLeft = lifeTime;
            _speed = speed;
            _params = projectileParams;
        }
        
        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            UpdatePosition();
            if (_timeLeft > 0) {
                return;
            }

            Projectile.TryHitTargetsInRadius(transform.position, _params.DamageRadius, _targetType, null, OnHit);
            PlayVfx(transform.position, Vector3.forward);
            Destroy();
        }
        
        private void OnHit(GameObject target)
        { 
            _hitCallback?.Invoke(target);

            if (target.TryGetComponent(out ExplosionReaction explosionReaction))
            {
                _explosionReactionParams.ExplosionPosition = transform.position;
                explosionReaction.OnExplosionReact(_explosionReactionParams);
            }
        }
        
        private void UpdatePosition()
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;
        }
        
        private void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.CreateObject(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
            vfx.transform.localScale *= _params.DamageRadius * _explosionScaleMultiplier;
        }
        
        private void Destroy()
        {
            gameObject.SetActive(false);
            _hitCallback = null;
            Destroy(gameObject);
        }
    }
}
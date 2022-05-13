using System;
using Survivors.Location.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Charge.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : Projectile
    {
        [SerializeField]
        private protected float _speed;
        [SerializeField]
        private float _maxLifeTime;
        [SerializeField]
        private GameObject _hitVfx;

        [Inject]
        private LocationObjectFactory _objectFactory;
        
        private float _timeLeft;

        public override void Launch(ITarget target, Action<GameObject> hitCallback)
        {
            base.Launch(target, hitCallback);
            SetupBullet();
        }

        private void SetupBullet()
        {
            _timeLeft = _maxLifeTime;
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            PlayVfx(hitPos, collisionNorm);
            Destroy();
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            UpdatePosition();
            if (_timeLeft > 0) {
                return;
            }
            Destroy();
        }
        private void UpdatePosition()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
        private void Destroy()
        {
            gameObject.SetActive(false);
            HitCallback = null;
            Destroy(gameObject);
        }

        private void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.CreateObject(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
    }
}
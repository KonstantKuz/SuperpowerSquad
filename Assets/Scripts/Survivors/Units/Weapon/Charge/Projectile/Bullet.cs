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

        protected Rigidbody Rigidbody { get; private set; }

        private float _timeLeft;

        public override void Launch(ITarget target, Action<GameObject> hitCallback)
        {
            base.Launch(target, hitCallback);
            SetupBullet();
        }

        protected void SetupBullet()
        {
            _timeLeft = _maxLifeTime;
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.velocity = transform.forward * _speed;
            Rigidbody.angularVelocity = Vector3.zero;
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
            if (_timeLeft > 0) {
                return;
            }
            Destroy();
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            HitCallback = null;
            Destroy(gameObject);
        }

        protected void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.CreateObject(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
    }
}
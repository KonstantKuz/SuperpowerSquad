using System;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : Projectile
    {
        [SerializeField]
        private float _maxLifeTime;
        [SerializeField]
        private GameObject _hitVfx;

        [Inject]
        private ObjectInstancingFactory _objectFactory;  
        
        private float _timeLeft;

        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            SetupBullet();
        }

        private void SetupBullet()
        {
            _timeLeft = _maxLifeTime;
        }
        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
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

        private void FixedUpdate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.fixedDeltaTime * Time.timeScale;
        }
        
        private void Destroy()
        {
            gameObject.SetActive(false);
            HitCallback = null;
            Destroy(gameObject);
        }

        protected void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.Create<MonoBehaviour>(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
    }
}
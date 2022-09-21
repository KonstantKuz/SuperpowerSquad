using System;
using Logger.Extension;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Location.Service;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        [SerializeField] private bool _spawnHitVfx;
        [SerializeField] private float _hitVfxScale;
        [SerializeField] private GameObject _hitVfx;
        
        [Inject]
        private ObjectInstancingFactory _objectFactory;
        
        public override void Fire(ITarget target, IProjectileParams chargeParams, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null)
            {
                this.Logger().Warn("Target is not a monobehaviour");
                return;
            }

            if (targetObj.GetComponent<IDamageable>() == null)
            {
                this.Logger().Warn("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);

            if (_spawnHitVfx)
            {
                PlayVfx(target.Center.position, -transform.forward);
            }
        }
        
        private void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.Create<MonoBehaviour>(_hitVfx);
            vfx.transform.localScale = _hitVfx.transform.localScale * _hitVfxScale;
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
    }
}
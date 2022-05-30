using System;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Player.Damageable
{
    public class DamageableChild : MonoBehaviour, IDamageable
    {
        private IDamageable _parentDamageable;
        public event Action OnDeath;
        public event Action OnDamageTaken;
        public bool DamageEnabled { get; set; } = true;
        private IDamageable ParentDamageable
        {
            get
            {
                if (_parentDamageable == null) {
                    _parentDamageable = transform.parent.gameObject.RequireComponentInParent<IDamageable>();
                    _parentDamageable.OnDeath += OnParentDeath;
                }
                return _parentDamageable;
            }
        }
        private void OnParentDeath()
        {
            _parentDamageable.OnDeath -= OnParentDeath;
            OnDeath?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            if (!DamageEnabled) {
                return;
            }
            ParentDamageable.TakeDamage(damage);
            OnDamageTaken?.Invoke();
        }

    }
}
using System;
using Feofun.Extension;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Player.Damageable
{
    public class DamageableChild : MonoBehaviour, IDamageable
    {
        private IDamageable _parentDamageable;
        public event Action OnZeroHealth;
        public event Action<float> OnDamageTaken;
        public bool DamageEnabled { get; set; } = true;
        public IDamageable ParentDamageable
        {
            get
            {
                if (_parentDamageable == null) {
                    _parentDamageable = transform.parent.gameObject.RequireComponentInParent<IDamageable>();
                    _parentDamageable.OnZeroHealth += OnParentZeroHealth;
                }
                return _parentDamageable;
            }
        }
        private void OnParentZeroHealth()
        {
            OnZeroHealth?.Invoke();
        }

        public void TakeDamage(float damage, DamageUnits units)
        {
            if (!DamageEnabled) {
                return;
            }
            ParentDamageable.TakeDamage(damage, units);
            OnDamageTaken?.Invoke(damage);
        }

        private void OnDestroy()
        {
            if (_parentDamageable != null)
            {
                _parentDamageable.OnZeroHealth -= OnParentZeroHealth;
            }
        }
    }
}
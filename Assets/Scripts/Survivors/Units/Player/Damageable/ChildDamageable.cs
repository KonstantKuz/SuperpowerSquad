using System;
using JetBrains.Annotations;
using Survivors.Squad.Component;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Player.Damageable
{
    public class ChildDamageable : MonoBehaviour, IDamageable
    {
        [CanBeNull]
        private IDamageable _parentDamageable;
        
        public event Action OnDeath;
        public event Action OnDamageTaken;
        public bool DamageEnabled { get; set; } = true;
        private IDamageable ParentDamageable => _parentDamageable ??= transform.parent.GetComponentInParent<IDamageable>();
        public void TakeDamage(float damage)
        {
            if (!DamageEnabled) {
                return;
            }
            ParentDamageable?.TakeDamage(damage);
            OnDamageTaken?.Invoke();
        }

    }
}
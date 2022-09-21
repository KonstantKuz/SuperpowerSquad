using System;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    public class FragileTarget : MonoBehaviour, IDamageable
    {
        public bool DamageEnabled { get; set; } = true;
        public event Action OnZeroHealth = delegate { };
        public event Action OnDamageTaken = delegate { };

        public void TakeDamage(float damage, DamageUnits units = DamageUnits.Value)
        {
            Destroy(gameObject);
        }
    }
}
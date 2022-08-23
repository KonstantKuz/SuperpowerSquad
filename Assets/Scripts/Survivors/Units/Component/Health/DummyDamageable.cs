using System;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    public class DummyDamageable : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage, DamageUnits units)
        {
        }

        public event Action OnZeroHealth = delegate { };
        public event Action OnDamageTaken = delegate { };
        public bool DamageEnabled { get; set; }
    }
}
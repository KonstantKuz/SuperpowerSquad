﻿using System;

namespace Survivors.Units.Component.Health
{
    public interface IDamageable
    { 
        void TakeDamage(float damage);
        event Action<DeathCause> OnDeath;
        event Action OnDamageTaken;
        bool DamageEnabled { get; set; } 
    }
}
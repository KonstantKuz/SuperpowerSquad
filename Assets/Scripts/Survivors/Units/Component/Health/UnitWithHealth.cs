using System;
using Survivors.Units.Model;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    public class UnitWithHealth : MonoBehaviour, IDamageable
    {
        private ReactiveProperty<float> _currentHealth;
        public IObservable<float> CurrentValue => _currentHealth;
        public int MaxValue => _healthConfig.MaxHealth;
        public bool DamageEnabled { get; set; }
        public event Action OnDeath;
        public event Action OnDamageTaken;
        
        private IUnitHealthModel _healthConfig;
      
        public void Init(IUnitHealthModel healthModel, Action onDeath)
        {
            _healthConfig = healthModel;
            _currentHealth = new FloatReactiveProperty(_healthConfig.MaxHealth);
            DamageEnabled = true;
            OnDeath += onDeath;
        }
        
        public void TakeDamage(float damage)
        {
            if (!DamageEnabled) {
                return;
            }
            ChangeHealth(-damage);
            LogDamage(damage);
            
            OnDamageTaken?.Invoke();
            if (_currentHealth.Value <= 0) {
                Die();
            }
        }
        
        private void Die()
        {
            DamageEnabled = false;
            OnDeath?.Invoke();
            OnDeath = null;
            OnDamageTaken = null;
        }
        
        private void ChangeHealth(float delta)
        {
            _currentHealth.Value = Mathf.Min(Mathf.Max(0, _currentHealth.Value + delta), MaxValue);
        }
        
        private void OnDestroy()
        {
            OnDeath = null;
            OnDamageTaken = null;
        }
        
        private void LogDamage(float damage)
        {
#if UNITY_EDITOR            
            Debug.Log($"Damage: -" + damage + " CurrentHealth: " + _currentHealth.Value + " GameObj:= " + gameObject.name);
#endif            
        }
    
    }
}
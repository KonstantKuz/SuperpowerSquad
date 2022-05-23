using System;
using Feofun.Extension;
using Survivors.Units.Model;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    public class UnitWithHealth : MonoBehaviour, IUnitInitializable, IDamageable, IHealthBarOwner
    {
        private IHealthModel _healthModel;
        private ReactiveProperty<float> _currentHealth;
        private IDisposable _disposable;
        
        public float MaxValue => _healthModel.MaxHealth.Value;
        public IObservable<float> CurrentValue => _currentHealth;
        public bool DamageEnabled { get; set; }
        
        public event Action OnDeath;
        public event Action OnDamageTaken;
        
        public void Init(IUnit unit)
        {
            _healthModel = unit.Model.HealthModel;
            _currentHealth = new FloatReactiveProperty(_healthModel.MaxHealth.Value);
            DamageEnabled = true;
            _disposable = _healthModel.MaxHealth.Diff().Subscribe(OnMaxHealthChanged);
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
            _disposable?.Dispose();
            _disposable = null;
        }
        
        private void LogDamage(float damage)
        {
#if UNITY_EDITOR            
            Debug.Log($"Damage: -" + damage + " CurrentHealth: " + _currentHealth.Value + " GameObj:= " + gameObject.name);
#endif            
        }

        private void OnMaxHealthChanged(float delta)
        {
            if (delta < 0) return;
            ChangeHealth(delta);
        }
    }
}
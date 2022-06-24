using System;
using Feofun.Extension;
using Logger.Assets.Scripts;
using Logger.Assets.Scripts.Extension;
using Survivors.Units.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Units.Component.Health
{
    public class Health : MonoBehaviour, IDamageable, IHealthBarOwner
    {
        
        private IHealthModel _healthModel;
        private ReactiveProperty<float> _currentHealth;
        private IDisposable _disposable;

        public float StartingMaxValue => _healthModel.StartingMaxHealth;
        public IReadOnlyReactiveProperty<float> MaxValue => _healthModel.MaxHealth;
        public IReadOnlyReactiveProperty<float> CurrentValue => _currentHealth;
        public bool DamageEnabled { get; set; }
        
        public event Action<DeathCause> OnDeath;
        public event Action OnDamageTaken;
        
        public void Init(IHealthModel health)
        {
            _healthModel = health;
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
            OnDeath?.Invoke(DeathCause.Killed);
            OnDeath = null;
            OnDamageTaken = null;
        }
        
        private void ChangeHealth(float delta, bool allowOverMax = false)
        {
            var newValue = Mathf.Max(0, _currentHealth.Value + delta);
            _currentHealth.Value = allowOverMax ?  newValue : Mathf.Min(newValue, MaxValue.Value);
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
            this.Logger().Trace($"Damage: -" + damage + " CurrentHealth: " + _currentHealth.Value + " GameObj:= " + gameObject.name);
#endif            
        }

        private void OnMaxHealthChanged(float delta)
        {
            if (delta < 0) return;
            ChangeHealth(delta);
        }

        public void Add(float value, bool allowOverMax = false)
        {
            Assert.IsTrue(value >= 0);
            ChangeHealth(value, allowOverMax);
        }
    }
}
using System;
using Feofun.Extension;
using Logger.Extension;
using Survivors.Units.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Component.Health
{
    public class Health : MonoBehaviour, IDamageable, IHealthBarOwner
    {
        private IHealthModel _healthModel;
        private ReactiveProperty<float> _currentHealth;
        private ReactiveProperty<float> _takenDamage;
        private IDisposable _disposable;

        public float StartingMaxValue => _healthModel.StartingMaxHealth;
        public IReadOnlyReactiveProperty<float> MaxValue => _healthModel.MaxHealth;
        public IReadOnlyReactiveProperty<float> CurrentValue => _currentHealth;
        
        public bool DamageEnabled { get; set; }
        public event Action OnZeroHealth;
        public event Action<float> OnDamageTaken;
        
        public void Init(IHealthModel health)
        {
            _healthModel = health;
            _currentHealth = new FloatReactiveProperty(_healthModel.MaxHealth.Value);
            _takenDamage = new FloatReactiveProperty();
            
            DamageEnabled = true;
            _disposable = _healthModel.MaxHealth.Diff().Subscribe(OnMaxHealthChanged);
        }
        
        public virtual void TakeDamage(float damage, DamageUnits units)
        {
            if (!DamageEnabled) {
                return;
            }
            if (units == DamageUnits.PercentFromMax) {
                damage = MaxValue.Value * damage / 100f;
            }
            
            ChangeHealth(-damage);
            LogDamage(damage);
            
            OnDamageTaken?.Invoke(damage);
            _takenDamage.SetValueAndForceNotify(damage);
            if (_currentHealth.Value <= 0) {
                OnZeroHealth?.Invoke();
            }
        }

        private void ChangeHealth(float delta, bool allowOverMax = false)
        {
            var newValue = Mathf.Max(0, _currentHealth.Value + delta);
            _currentHealth.Value = allowOverMax ?  newValue : Mathf.Min(newValue, MaxValue.Value);
        }
        
        private void OnDisable()
        {
            OnZeroHealth = null;
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

        public void Restore()
        {
            Add(_healthModel.MaxHealth.Value);
        }
    }
}
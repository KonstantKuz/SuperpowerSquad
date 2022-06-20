using System;
using Feofun.Components;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemySizeChanger : MonoBehaviour, IInitializable<IUnit>
    {
        private Health _health;
        private EnemyUnitModel _enemyModel;
        private Vector3 _initialScale;
        private EnemyScaleModel ScaleModel => _enemyModel.ScaleModel;
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private void Awake()
        {
            _initialScale = transform.localScale;
        }
        public void Init(IUnit unit)
        { 
            if (!(unit.Model is EnemyUnitModel enemyModel))
            {
                throw new ArgumentException($"Unit must be a enemy unit, gameObj:= {gameObject.name}");
            }
            _enemyModel = enemyModel;
            UpdateScale(ScaleModel.InitialScaleFactor);
            _health = gameObject.RequireComponent<Health>();
            _health.CurrentValue.Subscribe(it => OnHealthChanged()).AddTo(_disposable);
        }
        
        private void OnHealthChanged()
        {
            var currentHealth = _health.CurrentValue.Value;
            var level = _enemyModel.CalculateLevelOfHealth(currentHealth);
            UpdateScale(ScaleModel.GetScaleFactor(level));
        }
        private void UpdateScale(float scaleFactor)
        {
            transform.localScale = _initialScale * scaleFactor;
        }
        private void OnDestroy()
        {
            _disposable?.Dispose();
            _health.OnDamageTaken -= OnHealthChanged;
        }
    }
}
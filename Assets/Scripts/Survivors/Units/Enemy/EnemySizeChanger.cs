using System;
using Feofun.Components;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemySizeChanger : MonoBehaviour, IInitializable<IUnit>
    {
        private Health _health;
        private EnemyUnitModel _enemyModel;
        private Vector3 _initialScale;
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
            UpdateScale(enemyModel.ScaleFactor);
            _health = gameObject.RequireComponent<Health>();
            _health.OnDamageTaken += OnDamageTaken;
        }
        
        private void OnDamageTaken()
        {
            var currentHealth = _health.CurrentValue.Value;
            var level = currentHealth <= 0 ? EnemyUnitModel.MIN_LEVEL : (int) Mathf.Ceil(currentHealth / _enemyModel.Config.Health);
            UpdateScale(_enemyModel.CalculateScaleFactor(level));
        }
        private void UpdateScale(float scaleFactor)
        {
            transform.localScale = _initialScale * scaleFactor;
        }
        private void OnDestroy()
        {
            _health.OnDamageTaken -= OnDamageTaken;
        }
    }
}
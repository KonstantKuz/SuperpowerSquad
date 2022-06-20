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
        
        public void Init(IUnit unit)
        { 
            if (!(unit.Model is EnemyUnitModel enemyModel))
            {
                throw new ArgumentException($"Unit must be a enemy unit, gameObj:= {gameObject.name}");
            }
            _enemyModel = enemyModel;
            UpdateScale(_enemyModel.Level);
            _health = gameObject.RequireComponent<Health>();
            _health.OnDamageTaken += OnDamageTaken;
        }
        
        private void OnDamageTaken()
        {
            var currentHealth = _health.CurrentValue.Value;
            var level = _enemyModel.CalculateLevelOfHealth(currentHealth);
            UpdateScale(level);
        }
        private void UpdateScale(int level)
        {
            var scale = _enemyModel.CalculateScale(level);
            transform.localScale = Vector3.one * scale;
        }
        private void OnDestroy()
        {
            _health.OnDamageTaken -= OnDamageTaken;
        }
    }
}
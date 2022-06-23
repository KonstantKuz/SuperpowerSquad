﻿using Feofun.Components;
using Logger.Assets.Scripts;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Weapon;
using UnityEngine;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger<EnemyAttack>();
        
        private EnemyAi _enemyAi;
        private BaseWeapon _weapon;
        private EnemyAttackModel _attackModel;

        private float _attackTimer;

        
        public void Init(IUnit unit)
        {
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
        }

        public void OnTick()
        {
            UpdateTimer();
            if (CanAttack())
            {
                Attack();
            }
        }

        private void UpdateTimer()
        {
            _attackTimer += Time.deltaTime;
        }

        private bool CanAttack()
        {
            return _enemyAi.CurrentTarget != null 
                   && _enemyAi.DistanceToTarget <= _attackModel.AttackDistance 
                   && _attackTimer >= _attackModel.AttackInterval;
        }
        
        private void Attack()
        {
            _weapon.Fire(_enemyAi.CurrentTarget, null, DoDamage);
            _attackTimer = 0;
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            Logger.Trace($"Damage applied, target:= {target.name}");
        }
    }
}
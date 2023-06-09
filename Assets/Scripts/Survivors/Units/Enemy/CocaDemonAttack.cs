﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Feofun.Config;
using Feofun.Extension;
using Logger.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon.FormationWeapon;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Enemy
{
    public class CocaDemonAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IAimController
    {
        [SerializeField] private List<WeaponWithFormation> _weapons;

        private EnemyAi _enemyAi;
        private WeaponTimer _weaponTimer;
        private EnemyAttackModel _attackModel;
        private BossAttackConfig _currentAttackConfig;
        private Coroutine _currentAttack;
        private EnemyAnimationWrapper _enemyAnimationWrapper;
        private CompositeDisposable _disposable;

        [Inject]
        private ConfigCollection<ProjectileFormationType, BossAttackConfig> _attackConfigs;
        
        public bool IsNeedAim => _enemyAi.CurrentTarget != null && 
                                 _enemyAi.DistanceToTarget <= _attackModel.AttackDistance;
        
        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
            _weaponTimer = new WeaponTimer(_attackModel.AttackInterval);
            _weaponTimer.OnAttackReady += Attack;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _enemyAnimationWrapper = gameObject.RequireComponentInChildren<EnemyAnimationWrapper>();
        }

        public void OnTick()
        {
            if (_currentAttack != null) return;
            
            _weaponTimer.OnTick();
        }

        private void Attack()
        {
            if (!IsNeedAim) return;

            StopAttack();
            _currentAttackConfig = GetNextRandomAttackConfig();
            _currentAttack = StartCoroutine(Attack(_currentAttackConfig));
        }

        private BossAttackConfig GetNextRandomAttackConfig()
        {
            var randomAvailableType = _weapons.Random().FormationType;
            var randomConfig = _attackConfigs.Get(randomAvailableType);
            return randomConfig == _currentAttackConfig ? GetNextRandomAttackConfig() : randomConfig;
        }
        
        private IEnumerator Attack(BossAttackConfig attackConfig)
        {
            for (int i = 0; i < attackConfig.Count; i++)
            {
                _enemyAnimationWrapper.PlayAttack();
                yield return StartCoroutine(Fire());
                yield return new WaitForSeconds(attackConfig.Interval);
            }

            _currentAttack = null;
        }

        private IEnumerator Fire()
        {
            var target = _enemyAi.CurrentTarget; 
            target.OnTargetInvalid += StopAttack;
            Disposable.Create(() => target.OnTargetInvalid -= StopAttack).AddTo(_disposable);
            
            return _weapons.First(it => it.FormationType == _currentAttackConfig.Id)
                .Fire(target, 
                _currentAttackConfig.CreateProjectileParams(), 
                it => DoDamage(_currentAttackConfig.Damage, it));
        }

        private void DoDamage(float damage, GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(damage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            StopAttack();
            
            _disposable?.Dispose();
            _disposable = null;

            if (_weaponTimer != null)
            {
                _weaponTimer.OnAttackReady -= Attack;
            }
        }

        private void StopAttack()
        {
            if (_currentAttack != null)
            {
                StopCoroutine(_currentAttack);
                _currentAttack = null;
            }
        }
    }
}
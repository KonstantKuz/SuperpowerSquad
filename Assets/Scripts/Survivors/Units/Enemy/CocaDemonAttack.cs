using System.Collections;
using Feofun.Components;
using Feofun.Config;
using Feofun.Extension;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon.FormationWeapon;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Enemy
{
    public class CocaDemonAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IAttack
    {
        private EnemyAi _enemyAi;
        private WeaponWithFormation _weapon;
        private WeaponTimer _weaponTimer;
        private EnemyAttackModel _attackModel;
        private BossAttackConfig _currentAttackConfig;
        private Coroutine _currentAttack;
        private EnemyAnimationWrapper _enemyAnimationWrapper;

        [Inject]
        private ConfigCollection<ProjectileFormationType, BossAttackConfig> _attackConfigs;
        
        public bool CanAttack => _enemyAi.CurrentTarget != null && 
                                 _enemyAi.DistanceToTarget <= _attackModel.AttackDistance;        
        public void Init(IUnit unit)
        {
            Dispose();
            
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
            _weaponTimer = new WeaponTimer(_attackModel.AttackInterval);
            _weaponTimer.OnAttackReady += Attack;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _weapon = gameObject.RequireComponentInChildren<WeaponWithFormation>();
            _enemyAnimationWrapper = gameObject.RequireComponentInChildren<EnemyAnimationWrapper>();
        }

        public void OnTick()
        {
            if (_currentAttack != null) return;
            
            _weaponTimer.OnTick();
        }

        private void Attack()
        {
            if (!CanAttack) return;

            _currentAttackConfig = GetNextRandomAttackConfig();
            _currentAttack = StartCoroutine(Attack(_currentAttackConfig));
        }

        private BossAttackConfig GetNextRandomAttackConfig()
        {
            var randomConfig = _attackConfigs.Get(EnumExt.GetRandom<ProjectileFormationType>());
            return randomConfig == _currentAttackConfig ? GetNextRandomAttackConfig() : randomConfig;
        }
        
        private IEnumerator Attack(BossAttackConfig attackConfig)
        {
            for (int i = 0; i < attackConfig.Count; i++)
            {
                Fire();
                _enemyAnimationWrapper.PlayAttack();
                yield return new WaitForSeconds(attackConfig.Interval);
            }

            _currentAttack = null;
        }

        private void Fire()
        {
            _weapon.Fire(_currentAttackConfig.Id,
                _enemyAi.CurrentTarget, 
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
            _weapon.StopAttack();
            if (_weaponTimer != null)
            {
                _weaponTimer.OnAttackReady -= Attack;
            }
        }
    }
}
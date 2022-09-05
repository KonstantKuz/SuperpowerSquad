using Feofun.Components;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private EnemyAi _enemyAi;
        private BaseWeapon _weapon;
        private EnemyAttackModel _attackModel;
        private EnemyAnimationWrapper _enemyAnimationWrapper;

        private float _attackTimer;

        
        public void Init(IUnit unit)
        {
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _enemyAnimationWrapper = gameObject.RequireComponentInChildren<EnemyAnimationWrapper>();
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
                   && _attackTimer >= _attackModel.AttackInterval.Value;
        }
        
        private void Attack()
        {
            _enemyAnimationWrapper.PlayAttack();
            _weapon.Fire(_enemyAi.CurrentTarget, null, DoDamage);
            _attackTimer = 0;
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
    }
}
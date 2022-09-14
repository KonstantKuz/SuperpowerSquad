using Feofun.Components;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private EnemyAi _enemyAi;
        private BaseWeapon _weapon;
        private EnemyAttackModel _attackModel;
        private IProjectileParams _projectileParams;
        private WeaponTimer _weaponTimer;
        
        private EnemyAnimationWrapper enemyAnimationWrapper;
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;

        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;
        
        public void Init(IUnit unit)
        {
            Dispose();
            
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
            _projectileParams = _attackModel.CreateProjectileParams();
            _weaponTimer = new WeaponTimer(_attackModel.AttackInterval);
            _weaponTimer.OnAttackReady += Attack;
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            enemyAnimationWrapper = gameObject.RequireComponentInChildren<EnemyAnimationWrapper>();
            _weaponAnimationHandler = gameObject.GetComponentInChildren<WeaponAnimationHandler>();
        }

        public void OnTick()
        {
            _weaponTimer.OnTick();
        }

        public bool CanAttack()
        {
            return _enemyAi.CurrentTarget != null && 
                   _enemyAi.DistanceToTarget <= _attackModel.AttackDistance;
        }
        
        private void Attack()
        {
            if (!CanAttack()) {
                return;
            }
            enemyAnimationWrapper.PlayAttack();
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (!CanAttack()) {
                return;
            }
            _weapon.Fire(_enemyAi.CurrentTarget, _projectileParams, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void Dispose()
        {
            _weaponTimer.OnAttackReady -= Attack;
            if(HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
        }
    }
}
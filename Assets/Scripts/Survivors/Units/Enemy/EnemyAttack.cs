using Feofun.Components;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        private EnemyAi _enemyAi;
        private BaseWeapon _weapon;
        private EnemyAttackModel _attackModel;
        private WeaponTimer _weaponTimer;
        
        private Animator _animator;
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;

        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;
        
        public void Init(IUnit unit)
        {
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
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
            _animator = gameObject.RequireComponentInChildren<Animator>();
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
            _animator.SetTrigger(AttackHash);
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (!CanAttack()) {
                return;
            }
            _weapon.Fire(_enemyAi.CurrentTarget, null, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
    }
}
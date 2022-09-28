using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public abstract class EnemyAttackBase : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        [SerializeField] private BaseWeapon _weapon;
        [SerializeField] private bool _playAttackAnimation;

        protected EnemyAi _enemyAi;
        private EnemyAttackModel _attackModel;
        private IProjectileParams _projectileParams;
        private WeaponTimer _weaponTimer;

        private EnemyAnimationWrapper _enemyAnimationWrapper;
        [CanBeNull] private WeaponAnimationHandler _weaponAnimationHandler;

        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;
        private bool IsFireWithoutAnimation => HasWeaponAnimationHandler && !_playAttackAnimation;
        
        protected bool CanAttack => _enemyAi.CurrentTarget != null &&
                                    _enemyAi.DistanceToTarget <= _attackModel.AttackDistance;

        
        public abstract void Init(IUnit unit);
        
        protected void InitFire(EnemyAttackModel attackModel)
        {
            _attackModel = attackModel;
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
            _enemyAnimationWrapper = gameObject.RequireComponentInChildren<EnemyAnimationWrapper>();
            _weaponAnimationHandler = gameObject.GetComponentInChildren<WeaponAnimationHandler>();
        }

        public void OnTick()
        {
            _weaponTimer.OnTick();
        }

        private void Attack()
        {
            if (!CanAttack) {
                return;
            }
            if (_playAttackAnimation) {
                _enemyAnimationWrapper.PlayAttack();
            }
            if (!HasWeaponAnimationHandler || IsFireWithoutAnimation) {
                Fire();
            }
        }

        private void Fire()
        {
            if (!CanAttack) {
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

        protected void Dispose()
        {
            if (_weaponTimer != null) {
                _weaponTimer.OnAttackReady -= Attack;
                _weaponTimer = null;
            }
            if(HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IInitializable<Squad.Squad>
    {
        private static readonly int AttackSpeedMultiplierHash = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        
        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField]
        private string _attackAnimationName;
        
        private BaseWeapon _weapon;
        private PlayerAttackModel _playerAttackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private MovementController _movementController;
        private Unit _owner;
        private WeaponTimerManager _timerManager;
        
        
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;
        
        private bool IsTargetInvalid => !_target.IsTargetValidAndAlive();
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(IUnit unit)
        {
            _owner = (Unit) unit;
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            
            UpdateAnimationSpeed(_playerAttackModel.AttackInterval.Value);
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }
        public void Init(Squad.Squad owner)
        {
            _timerManager = owner.WeaponTimerManager;
            _timerManager.Subscribe(_owner.ObjectId, _playerAttackModel, OnAttackReady);
        }
        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }
        private void OnAttackReady()
        {
            if (CanAttack(_target)) {
                Attack();
            }
        }

        private void UpdateAnimationSpeed(float attackInterval)
        {
            var clips = _animator.runtimeAnimatorController.animationClips;
            var attackClipLength = clips.First(it => it.name == _attackAnimationName).length;
            if (attackInterval >= attackClipLength) {
                return;
            }
            _animator.SetFloat(AttackSpeedMultiplierHash, attackClipLength / attackInterval);
        }
        
        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        public void OnTick()
        {
            _target = FindTarget();
            if (_rotateToTarget) {
                _movementController.RotateToTarget(_target?.Center);
            }
        }
        private bool CanAttack([CanBeNull] ITarget target) => target != null;

        private void Attack()
        {
            _animator.SetTrigger(AttackHash);
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (IsTargetInvalid) {
                return;
            }
            _weapon.Fire(_target, _playerAttackModel.CreateProjectileParams(), DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
            _timerManager.Unsubscribe(_owner.ObjectId, OnAttackReady);
        }

      
    }
}
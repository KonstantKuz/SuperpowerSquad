using System;
using JetBrains.Annotations;
using ModestTree;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IUnitInitializable, IUpdatableUnitComponent, IAttack
    {
        private readonly int _attackHash = Animator.StringToHash("Attack");
        public event Action OnAttack;

        [SerializeField]
        private bool _rotateToTarget = true;

        private BaseWeapon _weapon;
        private PlayerAttackModel _playerAttackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private ReloadableWeaponTimer _reloadableWeaponTimer;
        private MovementController _movementController;

        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;

        private bool IsAttackProcess { get; set; }
        private bool IsTargetInvalid => !(_target is {IsAlive: true});
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(IUnit unit)
        {
            Assert.IsNull(_reloadableWeaponTimer);
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            _reloadableWeaponTimer = new ReloadableWeaponTimer(_playerAttackModel.ClipSize, _playerAttackModel.AttackTime, _playerAttackModel.ClipReloadTime, this);
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }

        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        public void OnTick()
        {
            var target = FindTarget();
            if (_rotateToTarget) {
                _movementController.RotateToTarget(target?.Center);
            }
            if (CanAttack(target)) {
                Attack(target);
            }
        }

        private bool CanAttack([CanBeNull] ITarget target) => target != null && _reloadableWeaponTimer.IsAttackReady && !IsAttackProcess;

        public void Attack(ITarget target)
        {
            IsAttackProcess = true;
            _target = target;
            _animator.SetTrigger(_attackHash);
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            IsAttackProcess = false;
            if (IsTargetInvalid) {
                return;
            }
            OnAttack?.Invoke();            
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
            _reloadableWeaponTimer.Dispose();
            _reloadableWeaponTimer = null;
        }
    }
}
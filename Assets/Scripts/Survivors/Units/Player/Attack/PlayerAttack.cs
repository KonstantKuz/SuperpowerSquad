using System;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Units.Damageable;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    public class PlayerAttack : MonoBehaviour, IUnitInitializable, IUpdatableUnitComponent, IAttack
    {
        private readonly int _attackHash = Animator.StringToHash("Attack");
        public event Action OnAttack;

        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField]
        private Transform _rotationRoot;
        [SerializeField]
        private float _rotationSpeed = 10;

        private BaseWeapon _weapon;
        private PlayerAttackModel _playerAttackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private ClipReloader _clipReloader;

        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;

        private bool IsAttackProcess { get; set; }
        private bool IsTargetInvalid => !(_target is {IsAlive: true});
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(IUnit unit)
        {
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            _clipReloader = new ClipReloader(_playerAttackModel.ClipSize, _playerAttackModel.AttackTime, _playerAttackModel.ClipReloadTime, this);
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _targetSearcher = GetComponent<ITargetSearcher>();

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }

        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        public void OnTick()
        {
            var target = FindTarget();
            if (_rotateToTarget) {
                UpdateRotation(target);
            }
            if (CanAttack(target)) {
                Attack(target);
            }
        }

        private bool CanAttack([CanBeNull] ITarget target) => target != null && _clipReloader.IsAttackReady && !IsAttackProcess;

        private void UpdateRotation([CanBeNull] ITarget target)
        {
            if (target != null) {
                RotateToTarget(target.Center.position);
            } else {
                Rotate(Quaternion.LookRotation(transform.forward));
            }
        }

        private void RotateToTarget(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - _rotationRoot.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, _rotationRoot.up);
            Rotate(lookAt);
        }

        private void Rotate(Quaternion lookAt)
        {
            _rotationRoot.rotation = Quaternion.Lerp(_rotationRoot.rotation, lookAt, Time.deltaTime * _rotationSpeed);
        }

        public void Attack(ITarget target)
        {
            IsAttackProcess = true;
            _target = target;
            _animator.SetTrigger(_attackHash);
            OnAttack?.Invoke();
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
            _clipReloader.Dispose();
            _clipReloader = null;
        }
    }
}
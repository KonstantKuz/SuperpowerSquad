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

        private BaseWeapon _weapon;
        private AttackModel _attackModel;
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

        public void Init(PlayerUnit playerUnit)
        {
            _attackModel = playerUnit.Model.AttackModel;
            _clipReloader = new ClipReloader(_attackModel.ClipSize, _attackModel.AttackTime, _attackModel.ClipReloadTime, this);
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
                _rotationRoot.rotation = Quaternion.Lerp(_rotationRoot.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 10);
            }
        }

        private void RotateToTarget(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - _rotationRoot.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, _rotationRoot.up);
            _rotationRoot.rotation = Quaternion.Lerp(_rotationRoot.rotation, lookAt, Time.deltaTime * 10);
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
            _weapon.Fire(_target, _attackModel.CreateChargeParams(), DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
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

        private class ClipReloader
        {
            private readonly float _attackInterval;
            private readonly float _reloadTime;
            private readonly int _clipSize;
            private readonly IAttack _attack;

            private int _currentClipSize;
            private float _lastAttackTime;
            private float _startReloadTime;
            private bool Reloaded => Time.time >= _startReloadTime + _reloadTime;
            public bool IsAttackReady => Time.time >= _lastAttackTime + _attackInterval && Reloaded;

            public ClipReloader(int clipSize, float attackTime, float reloadTime, IAttack attack)
            {
                _clipSize = clipSize;
                _currentClipSize = _clipSize;
                _reloadTime = reloadTime;
                _attackInterval = attackTime / clipSize;
                _attack = attack;
                attack.OnAttack += OnAttack;
            }

            public void Dispose()
            {
                _attack.OnAttack -= OnAttack;
            }

            private void OnAttack()
            {
                _lastAttackTime = Time.time;
                --_currentClipSize;
                if (_currentClipSize <= 0) {
                    Reload();
                }
            }

            private void Reload()
            {
                _startReloadTime = Time.time;
                _currentClipSize = _clipSize;
            }
        }
    }
}
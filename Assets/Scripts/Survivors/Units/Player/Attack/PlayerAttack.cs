using System;
using JetBrains.Annotations;
using ModestTree;
using Survivors.Extension;
using Survivors.Units.Damageable;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    public class PlayerAttack : MonoBehaviour, IUnitInitialization, IUpdatableUnitComponent
    {
        private readonly int _attackHash = Animator.StringToHash("Attack");

        private BaseWeapon _weapon;
        private AttackModel _attackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private MovementController _movementController;

        private float _rechargerCompletionTime;

        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private Recharger _recharger;
        [CanBeNull]
        private ITarget _target;
        private bool IsTargetInvalid => !(_target is {IsAlive: true});
        private bool IsAttackProcess { get; set; }
        private bool Recharged => Time.time >= _rechargerCompletionTime + _attackModel.RechargeTime;
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(PlayerUnit playerUnit)
        {
            _attackModel = playerUnit.Model.AttackModel;
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
                _weaponAnimationHandler.OnFireCompleted += FireCompleted;
            }
        }

        private void Awake()
        {
            _weapon = GetComponentInChildren<BaseWeapon>().IsNotNullComponent(this);
            _animator = GetComponentInChildren<Animator>().IsNotNullComponent(this);
            _movementController = GetComponent<MovementController>().IsNotNullComponent(this);
            _targetSearcher = GetComponent<ITargetSearcher>().IsNotNullComponent(this);

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }

        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        private void FireCompleted()
        {
            _movementController.PlayUnitRotateAnimation(0);
        }

        public void OnTick()
        {
            _recharger?.OnTick();

            if (_recharger != null) {
                return;
            }
            if (Recharged) {
                CreateRecharger();
            }
        }

        private void CreateRecharger()
        {
            _recharger = new Recharger(this, _attackModel.ChargeCount, _attackModel.AttackTime, OnRechargerCompleted);
        }

        private void OnRechargerCompleted()
        {
            _rechargerCompletionTime = Time.time;
            _recharger = null;
        }

        private void Attack(ITarget target)
        {
            IsAttackProcess = true;
            _target = target;

            RotateUnitToTarget(target);
            _animator.SetTrigger(_attackHash);
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void RotateUnitToTarget(ITarget target)
        {
            var targetDirection = target.Root.position - transform.position;
            var angle = Vector2.SignedAngle(transform.forward.ToVector2XZ(), targetDirection.ToVector2XZ());
            _movementController.PlayUnitRotateAnimation(angle);
        }

        private void Fire()
        {
            IsAttackProcess = false;
            if (IsTargetInvalid) {
                return;
            }
            _weapon.Fire(_target, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, $"IDamageable is null, gameObject:= {target.name}");
            damageable.TakeDamage(_attackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
                _weaponAnimationHandler.OnFireCompleted -= FireCompleted;
            }
            _recharger = null;
        }

        private class Recharger
        {
            private readonly PlayerAttack _attack;
            private readonly float _attackInterval;

            private int _chargeCount;
            private float _lastAttackTime;
            private Action _onCompleted;
            private bool IsAttackReady => Time.time >= _lastAttackTime + _attackInterval;

            public Recharger(PlayerAttack attack, int chargeCount, float attackTime, Action onCompleted)
            {
                _attack = attack;
                _onCompleted = onCompleted;
                _chargeCount = chargeCount;
                _attackInterval = attackTime / chargeCount;
            }

            public void OnTick()
            {
                if (_onCompleted == null) {
                    return;
                }
                if (!IsAttackReady) {
                    return;
                }
                if (_chargeCount <= 0) {
                    _onCompleted?.Invoke();
                    _onCompleted = null;
                    return;
                }
                var target = _attack.FindTarget();
                if (target != null) {
                    Attack(target);
                }
            }

            private void Attack(ITarget target)
            {
                _lastAttackTime = Time.time;
                _chargeCount--;
                _attack.Attack(target);
            }
        }
    }
}
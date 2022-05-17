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
    public class PlayerAttack : MonoBehaviour, IUnitInitializable, IUpdatableUnitComponent
    {
        private readonly int _attackHash = Animator.StringToHash("Attack");

        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField]
        private Transform _root;

        private BaseWeapon _weapon;
        private AttackModel _attackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;

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
            UpdateRotation(FindTarget());
            
            _recharger?.OnTick();

            if (_recharger != null) {
                return;
            }
            if (Recharged) {
                CreateRecharger();
            }
        }

        private void UpdateRotation([CanBeNull] ITarget target)
        {
            if (target != null) {
                if (_rotateToTarget) {
                    RotateToTarget(target.Center.position);
                }
               
            } else {
                _root.rotation = Quaternion.Lerp(_root.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 10);
            }
        }

        private void RotateToTarget(Vector3 targetPos)
        {
            var lookAtDirection = (targetPos - _root.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, _root.up);
            _root.rotation = Quaternion.Lerp(_root.rotation, lookAt, Time.deltaTime * 10);
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
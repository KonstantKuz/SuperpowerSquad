using System;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Units.Damageable;
using Survivors.Units.Player.Attack;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(ITargetSearcher))]
    public class EnemyAttack : MonoBehaviour
    {
        private BaseWeapon _weapon;
        private AttackModel _attackModel;
        private ITargetSearcher _targetSearcher;
        
        [CanBeNull]
        private ITarget _target;
        
        private bool IsTargetInvalid => !(_target is {IsAlive: true});

        public void Init(AttackModel attackModel)
        {
            _attackModel = attackModel;
        }

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _targetSearcher = GetComponent<ITargetSearcher>();
        }

        private void Update()
        {
            _target = _targetSearcher.Find();
            if (_target != null)
            {
                Fire();
            }
        }
        
        private void Fire()
        {
            _weapon.Fire(_target, null, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }
    }
}
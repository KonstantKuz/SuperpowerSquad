using System;
using System.Collections.Generic;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    public class CircularSawAttack : MonoBehaviour, IUnitInitializable, IUnitDeathEventReceiver
    {
        [SerializeField] private CircularSawWeapon _circularSawWeapon;

        private Unit _owner;
        private Squad.Squad _squad;
        private PlayerAttackModel _playerAttackModel;
        private CompositeDisposable _disposable;

        public void Init(IUnit unit)
        {
            if (_disposable != null)
            {
                Dispose();
            }
            _disposable = new CompositeDisposable();
            
            _owner = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackModel attackModel))
            {
                throw new ArgumentException("Unit must be a player unit.");
            }

            _playerAttackModel = attackModel;
            _squad = _owner.gameObject.RequireComponentInParent<Squad.Squad>();
            
            var projectileParams = attackModel.CreateProjectileParams();
            _circularSawWeapon.Init(_squad.Destination.transform, projectileParams);
            attackModel.ShotCount.Subscribe(CreateSaws).AddTo(_disposable);
        }

        public void OnDeath()
        {
            Dispose();
        }

        private void CreateSaws(int count)
        {
            _circularSawWeapon.CleanUp();
            for (int i = 0; i < count; i++)
            {
                _circularSawWeapon.AddSaw(_owner.SelfTarget.UnitType.GetTargetUnitType(), DoDamage);
            }
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private void Dispose()
        {
            _circularSawWeapon.CleanUp();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}

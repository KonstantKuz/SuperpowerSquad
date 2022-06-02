using System;
using Feofun.Components;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Player.Attack
{
    public class CircularSawAttack : MonoBehaviour, IInitializable<IUnit>, IUnitDeathEventReceiver, IInitializable<Squad.Squad>
    {
        [SerializeField] private CircularSawWeapon _circularSawWeapon;
        
        private Unit _ownerUnit;
        private Squad.Squad _squad;
        private PlayerAttackModel _attackModel;
        private CompositeDisposable _disposable;
        
        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _ownerUnit = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackModel attackModel))
            {
                throw new ArgumentException("Unit must be a player unit.");
            }
            _attackModel = attackModel;
        }
        public void Init(Squad.Squad squad)
        {
            Assert.IsNotNull(_ownerUnit);      
            Assert.IsNotNull(_attackModel);
            _squad = squad;
            var projectileParams = GetSawParamsForSquad();
            _circularSawWeapon.Init(_squad.Destination.transform, projectileParams);
            _attackModel.ShotCount.Subscribe(CreateSaws).AddTo(_disposable);
            _squad.UnitsCount.Subscribe(UpdateRadius).AddTo(_disposable);
        }
        
        private void CreateSaws(int count)
        {
            _circularSawWeapon.CleanUpSaws();
            var projectileParams = GetSawParamsForSquad();
            var targetType = _ownerUnit.SelfTarget.UnitType.GetTargetUnitType();
            for (int i = 0; i < count; i++)
            {
                _circularSawWeapon.AddSaw(targetType, projectileParams, DoDamage);
            }
        }

        private void UpdateRadius(int squadCount)
        {
            var projectileParams = GetSawParamsForSquad();
            _circularSawWeapon.OnParamsChanged(projectileParams);
        }

        private PlayerProjectileParams GetSawParamsForSquad()
        {
            var projectileParams = _attackModel.CreatePlayerProjectileParams();
            projectileParams.AdditionalAttackDistance += _squad.SquadRadius;
            return projectileParams;
        }

        public void OnDeath()
        {
            Dispose();
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private void Dispose()
        {
            _circularSawWeapon.CleanUp();
            _disposable?.Dispose();
            _disposable = null;
            _ownerUnit = null;
            _squad = null;
        }

    }
}

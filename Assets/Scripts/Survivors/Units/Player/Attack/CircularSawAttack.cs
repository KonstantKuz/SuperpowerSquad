using System;
using Feofun.Components;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Model.Session;
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
        private PlayerAttackSessionModel _attackSessionModel;
        private CompositeDisposable _disposable;
        
        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _ownerUnit = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackSessionModel attackModel))
            {
                throw new ArgumentException($"Unit must be a player unit, gameObj:= {gameObject.name}");
            }
            _attackSessionModel = attackModel;
        }
        public void Init(Squad.Squad squad)
        {
            Assert.IsNotNull(_ownerUnit);      
            Assert.IsNotNull(_attackSessionModel);
            _squad = squad;
            _attackSessionModel.ShotCount.Subscribe(CreateSaws).AddTo(_disposable);
            _squad.UnitsCount.Subscribe(UpdateRadius).AddTo(_disposable);
        }
        
        private void CreateSaws(int count)
        {
            var projectileParams = GetSawParamsForSquad();
            var targetType = _ownerUnit.TargetUnitType;
            _circularSawWeapon.Init(targetType, projectileParams, DoDamage);
        }

        private void UpdateRadius(int squadCount)
        {
            var projectileParams = GetSawParamsForSquad();
            _circularSawWeapon.OnParamsChanged(projectileParams);
        }

        private PlayerProjectileParams GetSawParamsForSquad()
        {
            var projectileParams = _attackSessionModel.CreatePlayerProjectileParams();
            projectileParams.AdditionalAttackDistance += _squad.SquadRadius;
            return projectileParams;
        }

        public void OnDeath(DeathCause deathCause)
        {
            Dispose();
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackSessionModel.AttackDamage);
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

using System;
using Feofun.Components;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    public class CircularSawAttack : MonoBehaviour, IInitializable<IUnit>, IUnitDeathEventReceiver
    {
        [SerializeField] private CircularSawWeapon _circularSawWeapon;

        [Inject]
        private World _world;
        
        private Unit _owner;
        private Squad.Squad _squad;
        private PlayerAttackModel _playerAttackModel;
        private CompositeDisposable _disposable;

        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _owner = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackModel attackModel))
            {
                throw new ArgumentException("Unit must be a player unit.");
            }

            _playerAttackModel = attackModel;
            _squad = _world.Squad;
            
            var projectileParams = GetSawParamsForSquad();
            _circularSawWeapon.Init(_squad.Destination.transform, projectileParams);
            attackModel.ShotCount.Subscribe(CreateSaws).AddTo(_disposable);
            _squad.UnitsCount.Subscribe(UpdateRadius).AddTo(_disposable);
        }

        private void CreateSaws(int count)
        {
            _circularSawWeapon.CleanUpSaws();
            for (int i = 0; i < count; i++)
            {
                _circularSawWeapon.AddSaw(_owner.SelfTarget.UnitType.GetTargetUnitType(), DoDamage);
            }
        }

        private void UpdateRadius(int squadCount)
        {
            var projectileParams = GetSawParamsForSquad();
            _circularSawWeapon.UpdateParams(projectileParams);
        }

        private ModifiableProjectileParams GetSawParamsForSquad()
        {
            var projectileParams = _playerAttackModel.CreateModifiableProjectileParams();
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

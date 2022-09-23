using Feofun.Components;
using Feofun.Extension;
using Logger.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class IceWaveAttack : MonoBehaviour, IInitializable<IUnit>, IInitializable<Squad.Squad>
    {
        [SerializeField] private IceWaveWeapon _iceWaveWeapon;

        private Unit _owner;
        private Squad.Squad _squad;
        private IWeaponTimerManager _weaponTimer;
        private PlayerAttackModel _playerAttackModel;
        
        public void Init(IUnit unit)
        {
            _owner = (Unit) unit;
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
        }
        
        public void Init(Squad.Squad squad)
        {
            _squad = squad;
            _weaponTimer = squad.WeaponTimerManager;
            _weaponTimer.Subscribe(_owner.ObjectId, _playerAttackModel, OnAttackReady);
        }

        private void OnAttackReady()
        {
            var parent = _squad.Destination.transform;
            var projectileParams = _playerAttackModel.CreateProjectileParams();
            var targetType = _owner.TargetUnitType;
            _iceWaveWeapon.Fire(parent, targetType, projectileParams, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            _weaponTimer.Unsubscribe(_owner.ObjectId, OnAttackReady);
        }
    }
}

using Feofun.Components;
using Survivors.Units.Player.Attack.Damager;
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
        private IDamager _damager;
        
        public void Init(IUnit unit)
        {
            _owner = (Unit) unit;
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            _damager = new PlayerDamager(_playerAttackModel);
        }
        
        public void Init(Squad.Squad squad)
        {
            _squad = squad;
            _weaponTimer = squad.WeaponTimerManager;
            _weaponTimer.Subscribe(_owner.ObjectId, _playerAttackModel, OnAttackReady);
        }

        private void OnAttackReady()
        {
            var parent = _squad.Center.transform;
            var projectileParams = _playerAttackModel.CreateProjectileParams();
            var targetType = _owner.TargetUnitType;
            _iceWaveWeapon.Fire(parent, targetType, projectileParams, _damager.Damage);
        }

        private void OnDestroy()
        {
            _weaponTimer.Unsubscribe(_owner.ObjectId, OnAttackReady);
        }
    }
}

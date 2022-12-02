using Survivors.Units.Player.Model;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack.Damager
{
    public class QueueWeaponDamager : PlayerDamager
    {
        private readonly int _divisionStep;
        private int _damagesCount;
        
        public QueueWeaponDamager(PlayerAttackModel attackModel, QueueWeapon weapon) : base(attackModel)
        {
            _divisionStep = weapon.QueueSize;
        }

        public override void Damage(GameObject target)
        {
            _damagesCount++;
            base.Damage(target);
        }

        protected override bool IsCritical()
        {
            return _damagesCount % _divisionStep == 0 && Random.value < _attackModel.CriticalChance;
        }
    }
}
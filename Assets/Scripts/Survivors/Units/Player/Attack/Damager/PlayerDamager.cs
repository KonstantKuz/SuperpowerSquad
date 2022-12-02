using Feofun.Extension;
using Logger.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using UnityEngine;

namespace Survivors.Units.Player.Attack.Damager
{
    public class PlayerDamager : IDamager
    {
        protected readonly PlayerAttackModel _attackModel;
        private float RegularDamage => _attackModel.AttackDamage;
        private float CriticalDamage => RegularDamage * _attackModel.CriticalMultiplier;
        
        public PlayerDamager(PlayerAttackModel attackModel)
        {
            _attackModel = attackModel;
        }

        public virtual void Damage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(GetDamage());
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private float GetDamage()
        {
            return IsCritical() ? CriticalDamage : RegularDamage;
        }

        protected virtual bool IsCritical()
        {
            return Random.value < _attackModel.CriticalChance;
        }
    }
}
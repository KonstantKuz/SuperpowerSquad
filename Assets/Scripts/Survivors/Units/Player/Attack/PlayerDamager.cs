using Feofun.Extension;
using Logger.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class PlayerDamager : IDamager
    {
        private readonly PlayerAttackModel _attackModel;
        public PlayerDamager(PlayerAttackModel attackModel)
        {
            _attackModel = attackModel;
        }

        public void Damage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            var damage = _attackModel.AttackDamage;
            if (Random.value < _attackModel.CriticalChance)
            {
                damage *= _attackModel.CriticalMultiplier;
            }
            damageable.TakeDamage(damage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
    }
}
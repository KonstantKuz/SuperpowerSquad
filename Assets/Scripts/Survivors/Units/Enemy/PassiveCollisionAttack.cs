using Feofun.Components;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class PassiveCollisionAttack : EnemyAttackBase
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _interval;
        [SerializeField] private float _additiveRadius;

        public override void Init(IUnit unit)
        {
            Dispose();
            _weapon = gameObject.RequireComponentInChildren<MeleeWeapon>();
            _attackModel = CreateAttackModel();
            InitFire();
        }

        private EnemyAttackModel CreateAttackModel()
        {
            return new EnemyAttackModel(_enemyAi.SelfRadius + _additiveRadius, _damage, _interval);
        }
    }
}
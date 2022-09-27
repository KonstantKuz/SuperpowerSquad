using Survivors.Units.Enemy.Model;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : EnemyAttackBase, IAimController
    {
        public bool IsNeedAim => CanAttack;
        public override void Init(IUnit unit)
        {
            Dispose();
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
            InitFire();
        }
    }
}
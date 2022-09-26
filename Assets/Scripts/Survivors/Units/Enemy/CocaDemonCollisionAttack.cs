using Feofun.Components;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Attack;
using Survivors.Units.Weapon;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class CocaDemonCollisionAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        [SerializeField] private float _interval;
        [SerializeField] private float _additiveRadius;
        
        private float _damage;
        private EnemyAi _enemyAi;
        private MeleeWeapon _meleeWeapon;
        private WeaponTimer _weaponTimer;

        private bool CanAttack => _enemyAi.CurrentTarget != null &&
                                  _enemyAi.DistanceToTarget <= _enemyAi.SelfRadius + _additiveRadius;

        public void Init(IUnit owner)
        {
            Dispose();

            _damage = owner.Model.AttackModel.AttackDamage;
            _weaponTimer = new WeaponTimer(new FloatReactiveProperty(_interval));
            _weaponTimer.OnAttackReady += Attack;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _meleeWeapon = gameObject.RequireComponentInChildren<MeleeWeapon>();
        }

        public void OnTick()
        {
            _weaponTimer.OnTick();
        }

        private void Attack()
        {
            if(!CanAttack) return;
            _meleeWeapon.Fire(_enemyAi.CurrentTarget, null, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_damage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void Dispose()
        {
            if (_weaponTimer != null)
            {
                _weaponTimer.OnAttackReady -= Attack;
            }
        }
    }
}
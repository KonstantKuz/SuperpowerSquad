using ModestTree;
using Survivors.Units.Damageable;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class PlayerAttack : MonoBehaviour, IUnitInitialization
    {
        private BaseWeapon _weapon;
        private AttackModel _attackModel;

        public void Init(Unit unit)
        {
            _attackModel = unit.UnitModel.AttackModel;
        }
        public void Awake()
        {
            _weapon = GetComponentInChildren<BaseWeapon>();
            Assert.IsNotNull(_weapon, "Unit prefab is missing BaseWeapon component in hierarchy");
        }

        private void Fire(ITarget target)
        {
            Assert.IsNotNull(_weapon, "BaseWeapon is missing");
            _weapon.Fire(target, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, $"IDamageable is null, gameObject:= {target.name}");
            damageable.TakeDamage(10);
        }

      
    }
}
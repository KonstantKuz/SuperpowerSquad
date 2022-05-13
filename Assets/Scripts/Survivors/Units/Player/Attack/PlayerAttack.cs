using EasyButtons;
using ModestTree;
using Survivors.Units.Damageable;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    public class PlayerAttack : MonoBehaviour, IUnitInitialization, IUpdatableUnitComponent
    {
        private readonly int _attackHash = Animator.StringToHash("Attack");

        [Inject]
        private TargetService _targetService;

        private BaseWeapon _weapon;
        private AttackModel _attackModel;
        private Animator _animator;
        private float _lastAttackTime;
        private bool IsReady => Time.time >= _lastAttackTime + _attackModel.AttackInterval;
        public void Init(PlayerUnit playerUnit)
        {
            _attackModel = playerUnit.UnitModel.AttackModel;
        }

        public void Awake()
        {
            _weapon = GetComponentInChildren<BaseWeapon>();
            _animator = GetComponentInChildren<Animator>();
            Assert.IsNotNull(_animator, "Unit prefab is missing Animator component in hierarchy");
            Assert.IsNotNull(_weapon, "Unit prefab is missing BaseWeapon component in hierarchy");
        }

        public void OnTick()
        {
            if (!IsReady) {
                return;
            }
            FindTargetAndAttack();
        }

        private ITarget FindTarget() =>
                _targetService.FindClosestTargetOfTypeByDistance(UnitType.ENEMY, transform.position, _attackModel.AttackDistance);

        private void FindTargetAndAttack()
        {
            var target = FindTarget();
            if (target == null) {
                return;
            } 
            Attack(target);
        }

        [Button]
        private void EditorAttack()
        {
            _animator.SetTrigger(_attackHash);
        }

        private void Attack(ITarget target)
        {
           // RotateTo(target.Root.position);
            _lastAttackTime = Time.time;
            _animator.SetTrigger(_attackHash);
            Fire(target);
        }
        
        /*private void RotateTo(Vector3 targetPos)
        {
            var transform = gameObject.transform;
            var lookAtDirection = (targetPos - transform.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * StateMachine._rotationSpeed);
        }*/
        
        private void Fire(ITarget target)
        {
            Debug.Log($"Damage applied, target:= {target.TargetId}");
            //_weapon.Fire(target, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, $"IDamageable is null, gameObject:= {target.name}");
            damageable.TakeDamage(_attackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }
    }
}
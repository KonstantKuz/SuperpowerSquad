using System.Linq;
using EasyButtons;
using JetBrains.Annotations;
using ModestTree;
using Survivors.Extension;
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
        private WeaponAnimationHandler _weaponAnimationHandler;

        private bool IsTargetInvalid => !(_target is {IsAlive: true});

        private ITarget _target;
        private bool IsAttackReady => Time.time >= _lastAttackTime + _attackModel.AttackInterval;
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(PlayerUnit playerUnit)
        {
            _attackModel = playerUnit.UnitModel.AttackModel;
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.FireEvent += Fire;
            }
        }

        public void Awake()
        {
            _weapon = GetComponentInChildren<BaseWeapon>();
            _animator = GetComponentInChildren<Animator>();
            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
            Assert.IsNotNull(_animator, "Unit prefab is missing Animator component in hierarchy");
            Assert.IsNotNull(_weapon, "Unit prefab is missing BaseWeapon component in hierarchy");
        }

        public void OnTick()
        {
            DrawDirection();
            if (!IsAttackReady) {
                return;
            }
            FindTargetAndAttack();
        }

        private void DrawDirection()
        {
            var it = _targetService.AllTargetsOfType(UnitType.ENEMY).First();
            var direction = it.Root.position - transform.position;
            DrawDebugRay(direction, Color.red);
            DrawDebugRay(transform.forward, Color.yellow);

            var angle = Vector2.Angle(transform.forward.ToVector2XZ(), direction.ToVector2XZ());
            Debug.Log($"Angle:= {angle}");
        }

        [CanBeNull]
        private ITarget FindTarget()
        {
            return _targetService.AllTargetsOfType(UnitType.ENEMY)
                                 .Where(it => Vector3.Distance(it.Root.position, transform.position) <= _attackModel.AttackDistance)
                                 .Where(it => {
                                     var direction = it.Root.position - transform.position;
                                     return Vector2.Angle(transform.forward.ToVector2XZ(), direction.ToVector2XZ()) <= _attackModel.AttackAngle * 0.5f;
                                 })
                                 .OrderBy(it => Vector3.Distance(it.Root.position, transform.position))
                                 .FirstOrDefault();
        }

        private void DrawDebugRay(Vector3 rayDirection, Color color)
        {
            Vector3 debugRay = rayDirection * 10;
            Debug.DrawRay(transform.position, debugRay, color, .1f, false);
        }

        private void FindTargetAndAttack()
        {
            _target = FindTarget();
            if (_target == null) {
                return;
            }
            Attack();
        }

        [Button]
        private void EditorAttack()
        {
            _animator.SetTrigger(_attackHash);
        }

        private void Attack()
        {
            _lastAttackTime = Time.time;
            if (!HasWeaponAnimationHandler) {
                Fire();
            }

            _animator.SetTrigger(_attackHash);
        }

        /*private void RotateTo(Vector3 targetPos)
        {
            var transform = gameObject.transform;
            var lookAtDirection = (targetPos - transform.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * 20);
        }*/

        private void Fire()
        {
            if (IsTargetInvalid) {
                _lastAttackTime = 0;
                return;
            }
            _weapon.Fire(_target, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, $"IDamageable is null, gameObject:= {target.name}");
            damageable.TakeDamage(_attackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            _weaponAnimationHandler.FireEvent -= Fire;
        }
    }
}
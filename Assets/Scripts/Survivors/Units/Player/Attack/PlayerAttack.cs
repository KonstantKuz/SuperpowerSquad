using System.Linq;
using Feofun.Components;
using JetBrains.Annotations;
using Logger.Assets.Scripts;
using ModestTree;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using Survivors.Units.Target;
using Survivors.Units.Weapon;
using UnityEngine;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger<PlayerAttack>();
        
        private static readonly int AttackSpeedMultiplierHash = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        
        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField] private string _attackAnimationName;
        
        private BaseWeapon _weapon;
        private PlayerAttackModel _playerAttackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private ReloadableWeaponTimer _weaponTimer;
        private MovementController _movementController;

        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;
        
        private bool IsTargetInvalid => !_target.IsTargetValidAndAlive();
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        public void Init(IUnit unit)
        {
            Assert.IsNull(_weaponTimer);
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            _weaponTimer = new ReloadableWeaponTimer(_playerAttackModel.ClipSize, _playerAttackModel.AttackTime, _playerAttackModel.ClipReloadTime);
            UpdateAnimationSpeed(_weaponTimer.AttackInterval);
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        private void UpdateAnimationSpeed(float attackInterval)
        {
            var clips = _animator.runtimeAnimatorController.animationClips;
            var attackClipLength = clips.First(it => it.name == _attackAnimationName).length;
            if (attackInterval >= attackClipLength) {
                return;
            }
            _animator.SetFloat(AttackSpeedMultiplierHash, attackClipLength / attackInterval);
        }

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }

        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        public void OnTick()
        {
            var target = FindTarget();
            if (_rotateToTarget) {
                _movementController.RotateToTarget(target?.Center);
            }
            if (CanAttack(target)) {
                Attack(target);
            }
        }

        private bool CanAttack([CanBeNull] ITarget target) => target != null && _weaponTimer.IsAttackReady;

        private void Attack(ITarget target)
        {
            _target = target;
            _animator.SetTrigger(AttackHash);
            _weaponTimer.OnAttack();            
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (IsTargetInvalid)
            {
                _weaponTimer.CancelLastTimer();
                return;
            }
            _weapon.Fire(_target, _playerAttackModel.CreateProjectileParams(), DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            _logger.Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
            _weaponTimer = null;
        }
    }
}
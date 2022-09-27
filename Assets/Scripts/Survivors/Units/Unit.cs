using System;
using EasyButtons;
using Feofun.Components;
using Feofun.Extension;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.Location.Model;
using Survivors.Units.Component;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Messages;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;
using Survivors.Units.Model;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using UnityEngine;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit, IMovementLockable
    {
        private IUpdatableComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        private IUnitDeathEventReceiver[] _deathEventReceivers;
        private IUnitActiveStateReceiver[] _activeStateReceiver;
        private MovementController _movementController;
        private bool _isActive;
        private int _lockCount;
        private float _spawnTime;
        private Collider _collider;

        [Inject]
        private IMessenger _messenger;
        [Inject]
        private UnitService _unitService;
        [Inject]
        private UpdateManager _updateManager;

        public bool IsActive
        {
            get => _isActive;
            set {
                if (_isActive == value) {
                    return;
                }
                _isActive = value;
                _activeStateReceiver.ForEach(it => it.OnActiveStateChanged(_isActive));
            }
        }

        public UnitType UnitType => _selfTarget.UnitType;
        public UnitType TargetUnitType => _selfTarget.UnitType.GetTargetUnitType();
        public ITarget SelfTarget => _selfTarget;
        public IUnitModel Model { get; private set; }
        public event Action<IUnit, DeathCause> OnDeath;
        public event Action<IUnit> OnUnitDestroyed;
        public MovementController MovementController => _movementController ??= GetComponent<MovementController>();

        public float LifeTime => Time.time - _spawnTime;
        [CanBeNull]
        public Health Health { get; private set; }
        public Bounds Bounds => _collider.bounds;

        private void Awake()
        {
            _updatables = GetComponentsInChildren<IUpdatableComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death = gameObject.RequireComponent<IUnitDeath>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();
            _activeStateReceiver = GetComponentsInChildren<IUnitActiveStateReceiver>();    
            Health = GetComponent<Health>();
            _collider = GetComponent<CapsuleCollider>();
            
        }
        public void Init(IUnitModel model)
        {
            Model = model;
            
            if (UnitType == UnitType.ENEMY)
            {
                _damageable.OnDamageTaken += OnDamageTaken;
                _damageable.OnZeroHealth += DieOnZeroHealth;
            }

            IsActive = true;
            _spawnTime = Time.time;
            
            foreach (var component in GetComponentsInChildren<IInitializable<IUnit>>()) {
                component.Init(this);
            }
            _unitService.Add(this);
            _updateManager.StartUpdate(UpdateComponents);
        }

        private void OnDamageTaken(float damage)
        {
            _messenger.Publish(new EnemyDamagedMessage(this, damage));
        }

        public void Lock()
        {
            _lockCount++;
            IsActive = false;
        }

        public void UnLock()
        {
            if (_lockCount > 0) {
                _lockCount--;
            }
            if (_lockCount <= 0) {
                IsActive = true;
            }
        }
        [Button]
        public void Kill(DeathCause deathCause)
        {
            _damageable.DamageEnabled = false;
            _damageable.OnDamageTaken -= OnDamageTaken;
            _damageable.OnZeroHealth -= DieOnZeroHealth;
            IsActive = false;
            _deathEventReceivers.ForEach(it => it.OnDeath(deathCause));
            OnDeath?.Invoke(this, deathCause);
            OnDeath = null;
            _death.PlayDeath();
   
        }

        private void DieOnZeroHealth()
        {
            Kill(DeathCause.Killed);
        }

        private void UpdateComponents()
        {
            if (!IsActive) {
                return;
            }
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }
        private void OnDisable()
        {
            OnUnitDestroyed?.Invoke(this);
            OnUnitDestroyed = null;
            _unitService.Remove(this);
            _updateManager.StopUpdate(UpdateComponents);
        }
        public void AddModifier(IModifier modifier)
        {
            if (!(Model is PlayerUnitModel playerUnitModel)) {
                this.Logger().Error($"Unit model must be the PlayerUnitModel, current model:= {Model.GetType().Name}");
                return;
            }
            playerUnitModel.AddModifier(modifier);
        }
    }
}
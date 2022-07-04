using System;
using Survivors.Extension;
using EasyButtons;
using Feofun.Components;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.App;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;
using Survivors.Units.Model;
using Survivors.Units.Player.Model.Session;
using Survivors.Units.Player.Movement;
using UnityEngine;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit
    {
        private IUpdatableComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        private IUnitDeathEventReceiver[] _deathEventReceivers;
        private IUnitDeactivateEventReceiver[] _deactivateEventReceivers;
        private MovementController _movementController;
        private bool _isActive;
        private float _spawnTime;
        private Collider _collider;

        [Inject]
        private UnitService _unitService;
        [Inject]
        private UpdateManager _updateManager;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (!_isActive) {
                    _deactivateEventReceivers.ForEach(it => it.OnDeactivate());
                }
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

        public void Init(IUnitModel model)
        {
            Model = model;

            _updatables = GetComponentsInChildren<IUpdatableComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death = gameObject.RequireComponent<IUnitDeath>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();
            _deactivateEventReceivers = GetComponentsInChildren<IUnitDeactivateEventReceiver>();

            _damageable.OnDeath += Kill;
            IsActive = true;
            _spawnTime = Time.time;
            Health = GetComponent<Health>();
            _collider = GetComponent<CapsuleCollider>();

            foreach (var component in GetComponentsInChildren<IInitializable<IUnit>>()) {
                component.Init(this);
            }

            _unitService.Add(this);
            _updateManager.StartUpdate(UpdateComponents);
        }

        [Button]
        public void Kill(DeathCause deathCause)
        {
            _damageable.OnDeath -= Kill;
            IsActive = false;
            _deathEventReceivers.ForEach(it => it.OnDeath(deathCause));
            _death.PlayDeath();
            OnDeath?.Invoke(this, deathCause);
            OnDeath = null;
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

        private void OnDestroy()
        {
            OnUnitDestroyed?.Invoke(this);
            OnUnitDestroyed = null;
            _unitService.Remove(this);
            _updateManager.StopUpdate(UpdateComponents);
        }

        public void AddModifier(IModifier modifier)
        {
            if (!(Model is PlayerUnitModel sessionModel)) {
                this.Logger().Error($"Unit model must be the PlayerUnitSessionModel, current model:= {Model.GetType().Name}");
                return;
            }
            sessionModel.AddModifier(modifier);
        }
    }
}
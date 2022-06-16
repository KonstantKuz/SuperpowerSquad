using System;
using Survivors.Extension;
using EasyButtons;
using Feofun.Components;
using Feofun.Modifiers;
using SuperMaxim.Core.Extensions;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;
using Survivors.Units.Model;
using Survivors.Units.Player.Movement;
using UnityEngine;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit
    {
        [Inject]
        private UnitService _unitService;

        private IUpdatableComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        private IUnitDeathEventReceiver[] _deathEventReceivers;   
        private IUnitDeactivateEventReceiver[] _deactivateEventReceivers;
        private MovementController _movementController;
        private bool _isActive;
        private float _spawnTime;

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
        public IUnitModel Model { get; private set; }
        public event Action<IUnit> OnDeath;
        public MovementController MovementController => _movementController ??= GetComponent<MovementController>();

        public float LifeTime => Time.time - _spawnTime;
        
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
            _unitService.Add(this);
            IsActive = true;
            _spawnTime = Time.time;
            
            foreach (var component in GetComponentsInChildren<IInitializable<IUnit>>()) {
                component.Init(this);
            }
        }
        
        [Button]
        public void Kill()
        {
            _damageable.OnDeath -= Kill;
            IsActive = false;
            _deathEventReceivers.ForEach(it => it.OnDeath());
            _death.PlayDeath();
            OnDeath?.Invoke(this);
            OnDeath = null;
        }

        private void Update()
        {
            if (!IsActive) {
                return;
            }
            UpdateComponents();
        }

        private void UpdateComponents()
        {
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }

        private void OnDestroy()
        {
            _unitService.Remove(this);
        }

        public void AddModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }
    }
}
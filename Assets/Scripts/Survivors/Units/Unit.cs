using System;
using Survivors.Extension;
using EasyButtons;
using Feofun.Components;
using Feofun.Modifiers;
using SuperMaxim.Core.Extensions;
using Survivors.App;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;
using Survivors.Units.Model;
using Survivors.Units.Player.Movement;

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

        [Inject] private UnitService _unitService;
        [Inject] private UpdateManager _updateManager;

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
            
            foreach (var component in GetComponentsInChildren<IInitializable<IUnit>>()) {
                component.Init(this);
            }
            
            _unitService.Add(this);
            _updateManager.StartUpdate(UpdateComponents);
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
            _unitService.Remove(this);
            _updateManager.StopUpdate(UpdateComponents);
        }

        public void AddModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }
    }
}
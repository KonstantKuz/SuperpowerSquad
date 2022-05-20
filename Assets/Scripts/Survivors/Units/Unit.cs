using System;
using Survivors.Extension;
using EasyButtons;
using SuperMaxim.Core.Extensions;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;
using Survivors.Units.Model;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit
    {
        [Inject]
        private UnitService _unitService;

        private IUpdatableUnitComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        private IUnitDeathEventReceiver[] _deathEventReceivers;
      
        public event Action<IUnit> OnDeath;
        public IUnitModel Model { get; private set; }
        
        public GameObject Object => gameObject;
        public UnitType UnitType => _selfTarget.UnitType;
        
        public bool IsAlive { get; set; }
        public void Init(IUnitModel model)
        {
            Model = model;

            foreach (var component in GetComponentsInChildren<IUnitInitializable>()) {
                component.Init(this);
            }
            _updatables = GetComponentsInChildren<IUpdatableUnitComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death = gameObject.RequireComponent<IUnitDeath>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();

            _damageable.OnDeath += Kill;
            _unitService.Add(this);
            IsAlive = true;
        }
        
        [Button]
        public void Kill()
        {
            _damageable.OnDeath -= Kill;
            IsAlive = false;
            _deathEventReceivers.ForEach(it => it.OnDeath());
            _death.PlayDeath();
            OnDeath?.Invoke(this);
            OnDeath = null;
        }

        private void Update()
        {
            if (!IsAlive) {
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
    }
}
using System;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units
{
    public interface IUnitDeathEventReceiver
    {
        void OnDeath();
    }
    
    public class Unit : WorldObject, IUnit
    {
        private IUpdatableUnitComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private IUnitDeathEventReceiver[] _deathEventReceivers;
      
        public IUnitModel Model { get; private set; }
        public GameObject GameObject => gameObject;

        public Action<IUnit> OnDeath { get; set; }

        public void Init(IUnitModel model)
        {
            Model = model;

            foreach (var component in GetComponentsInChildren<IUnitInitializable>()) {
                component.Init(this);
            }
            _updatables = GetComponentsInChildren<IUpdatableUnitComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death =  gameObject.RequireComponent<IUnitDeath>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();

            _damageable.OnDeath += Kill;
        }

        public void Kill()
        {
            _damageable.OnDeath -= Kill;
            _deathEventReceivers.ForEach(it => it.OnDeath());
            
            _death.PlayDeath();
            OnDeath?.Invoke(this);
            OnDeath = null;
        }

        private void Update()
        {
            UpdateComponents();
        }

        private void UpdateComponents()
        {
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }
    }
}
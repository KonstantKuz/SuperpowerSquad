using System;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit
    {
        private IUpdatableUnitComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        public IUnitModel Model { get; private set; }

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
            _selfTarget =  gameObject.RequireComponent<ITarget>();
            
            _damageable.OnDeath += Kill;
        }

        public void Kill()
        {
            _damageable.OnDeath -= Kill;

            _death.PlayDeath();
            
            _selfTarget.OnTargetInvalid?.Invoke(_selfTarget);
            
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
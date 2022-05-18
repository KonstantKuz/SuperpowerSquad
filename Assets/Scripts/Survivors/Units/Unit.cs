using System;
using EasyButtons;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Death;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

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

        public event Action<IUnit> OnDeath;
        public IUnitModel Model { get; private set; }
        public GameObject Object => gameObject;
        public UnitType UnitType => _selfTarget.UnitType;

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

            _damageable.OnDeath += Kill;
            _unitService.Add(this);
        }

        [Button]
        public void KillUnit()
        {
            Kill();
        }

        public void Kill()
        {
            _damageable.OnDeath -= Kill;
            _death.PlayDeath();
            _selfTarget.OnDeath();
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

        private void OnDestroy()
        {
            _unitService.Remove(this);
        }
    }
}
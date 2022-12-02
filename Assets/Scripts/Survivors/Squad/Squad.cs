using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Feofun.Components;
using Feofun.Config;
using Feofun.Extension;
using Feofun.Modifiers;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Loot;
using Survivors.Modifiers;
using Survivors.Squad.Component;
using Survivors.Squad.Formation;
using Survivors.Squad.Model;
using Survivors.Units;
using Survivors.Units.Component;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Attack;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Zenject;
using Unit = Survivors.Units.Unit;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour, IWorldScope, IMovementLockable
    {
        private readonly IReactiveCollection<Unit> _units = new List<Unit>().ToReactiveCollection();
        
        private IDamageable _damageable;
        private IReadOnlyReactiveProperty<int> _unitCount;
        private LootCollector _lootCollector;
        private SquadFormationController _formationController;
        
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private UnitFactory _unitFactory;
        
        public bool IsActive { get; set; }
        public SquadModel Model { get; private set; }
        public Health Health { get; private set; }
        public SquadCenter Center { get; private set; }
        public SquadTargetProvider TargetProvider { get; private set; }
        public WeaponTimerManager WeaponTimerManager { get; private set; }

        public IReactiveCollection<Unit> Units => _units;
        public IReadOnlyReactiveProperty<int> UnitsCount => _unitCount ??= _units.ObserveCountChanged().ToReactiveProperty();

        public float SquadRadius { get; private set; }
        public Vector3 Position => Center.transform.position;

        public event Action OnZeroHealth;
        public event Action OnDeath;

        public void Awake()
        {
            _formationController = gameObject.RequireComponent<SquadFormationController>();
            Health = gameObject.RequireComponent<Health>();
            Center = gameObject.RequireComponentInChildren<SquadCenter>();
            TargetProvider = gameObject.RequireComponent<SquadTargetProvider>();   
            WeaponTimerManager = gameObject.RequireComponent<WeaponTimerManager>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _lootCollector = gameObject.RequireComponentInChildren<LootCollector>();
        }

        public void Init(SquadModel model)
        {
            Model = model;
            IsActive = true;

            InitializeSquadComponents(gameObject);
            _damageable.OnZeroHealth += OnZeroHealth;
        }
        
        private void InitializeSquadComponents(GameObject owner)
        {
            foreach (var component in owner.GetComponentsInChildren<IInitializable<Squad>>()) {
                component.Init(this);
            }
        }
        
        public void Lock()
        {
            IsActive = false;
        }

        public void UnLock()
        {
            IsActive = true;
        }

        public void OnWorldSetup()
        {
        }
        public void OnWorldCleanUp()
        {
            _units.Clear();
            Model = null;
        }

        public void AddUnit(Unit unit)
        {
            unit.transform.SetParent(Center.transform);
            Model.AddUnit(unit.Model);
            _units.Add(unit);
            InitializeSquadComponents(unit.gameObject);
            SquadRadius = _formationController.CalculateSquadRadius();
        }

        public void RemoveUnits()
        {
            _units.ForEach(it => {
                it.Kill(DeathCause.Removed);
                Destroy(it.gameObject);
            });
            _units.Clear();
            Model.ResetHealth();
        }

        public void Kill()
        {
            IsActive = false;
            _damageable.OnZeroHealth -= OnZeroHealth;
            OnDeath?.Invoke();
            _units.ForEach(it => it.Kill(DeathCause.Killed));
            _units.Clear();
        }

        public void AddModifier(IModifier modifier, ModifierTarget target, [CanBeNull] string unitId = null)
        {
            switch (target)
            {
                case ModifierTarget.Unit:
                    AddUnitModifier(modifier, unitId);
                    break;
                case ModifierTarget.Squad:
                    Assert.IsNull(unitId);
                    AddSquadModifier(modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void AddSquadModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }

        private void AddUnitModifier(IModifier modifier, [CanBeNull] string unitId = null)
        {
            var units = _units.ToList();
            if (unitId != null) units = _units.Where(it => it.Model.Id == unitId).ToList();
            units.ForEach(unit => unit.AddModifier(modifier));
        }
        
        public void RestoreHealth()
        {
            Health.Restore();
        }
        [Button]
        /*
         * This functions just tests formation change when new units are added
         */
        public void SpawnUnit()
        {
            Assert.IsTrue(_units.Count > 0);
            var nextUnit = _playerUnitConfigs.Values[_units.Count % _playerUnitConfigs.Values.Count];
            _unitFactory.CreatePlayerUnit(nextUnit.Id);
        }

        [Button]
        private void SwitchSquadCenterVisibility()
        {
            Center.SwitchVisibility();
        }
    }
}
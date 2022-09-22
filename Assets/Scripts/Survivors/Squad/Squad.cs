﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Feofun.Components;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
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
        [SerializeField] private float _unitSize;   
        [SerializeField] private float _destinationLimit = 1000;
        
        private ISquadFormation _formation;
        private readonly IReactiveCollection<Unit> _units = new List<Unit>().ToReactiveCollection();
        
        private IDamageable _damageable;
        private IReadOnlyReactiveProperty<int> _unitCount;
        private LootCollector _lootCollector;
        
        [Inject] private Joystick _joystick;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;
        
        public bool IsActive { get; set; }
        public SquadModel Model { get; private set; }
        public Health Health { get; private set; }
        public SquadDestination Destination { get; private set; }
        public SquadTargetProvider TargetProvider { get; private set; }
        public WeaponTimerManager WeaponTimerManager { get; private set; }
        public IReadOnlyReactiveProperty<int> UnitsCount => _unitCount ??= _units.ObserveCountChanged().ToReactiveProperty();
        public float SquadRadius { get; private set; }
        public bool IsMoving => _joystick.Direction.sqrMagnitude > 0;
        public Vector3 MoveDirection => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        public Vector3 Position => Destination.transform.position;

        public event Action OnZeroHealth;
        public event Action OnDeath;
        
        public void Init(SquadModel model)
        {
            Model = model;
            _damageable.OnZeroHealth += CheckRespawn;
            InitializeSquadComponents(gameObject);
            IsActive = true;
        }        

        public void Awake()
        {
            _formation = new FilledCircleFormation();
            Health = gameObject.RequireComponent<Health>();
            Destination = gameObject.RequireComponentInChildren<SquadDestination>();
            TargetProvider = gameObject.RequireComponent<SquadTargetProvider>();   
            WeaponTimerManager = gameObject.RequireComponent<WeaponTimerManager>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _lootCollector = gameObject.RequireComponentInChildren<LootCollector>();
            UpdateSquadRadius();
        }

        public void Lock()
        {
            IsActive = false;
        }

        public void UnLock()
        {
            IsActive = true;
        }
        private void Update()
        {
            if (!IsActive) return;
            if (IsMoving) Move(MoveDirection);
            UpdateUnitsAnimations();
            _units.ForEach(it => it.transform.localPosition = Vector3.zero);
        }
        
        public void OnWorldSetup()
        {
        }

        public void OnWorldCleanUp()
        {
            _units.Clear();
            Model = null;
        }

        private void CheckRespawn()
        {
            OnZeroHealth?.Invoke();
        }
        
        public void Kill()
        {
            IsActive = false;
            _damageable.OnZeroHealth -= CheckRespawn;
            OnDeath?.Invoke();
            _units.ForEach(it => it.Kill(DeathCause.Killed));
            _units.Clear();
        }

        public void RemoveUnits()
        {
            _units.ForEach(it => {
                it.Kill(DeathCause.Removed);
                Destroy(it.gameObject);
            });
            _units.Clear();
            Model.OnRemoveUnits();
        }

        public void AddUnit(Unit unit)
        {
            unit.transform.SetParent(Destination.transform);
            Model.AddUnit(unit.Model);
            _units.Add(unit);
            InitializeSquadComponents(unit.gameObject);
            UpdateSquadRadius();
            DisableUnitView(unit);
        }

        private void DisableUnitView(Unit unit)
        {
            if (_units.Count == 1)
            {
                return;
            }
            unit.GetComponent<NavMeshAgent>().enabled = false;
            unit.GetComponent<Collider>().enabled = false;
            var renderers = unit.GetComponentsInChildren<Renderer>();
            renderers.ForEach(it => it.enabled = false);
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
        
        private void InitializeSquadComponents(GameObject owner)
        {
            foreach (var component in owner.GetComponentsInChildren<IInitializable<Squad>>()) {
                component.Init(this);
            }
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
            Destination.SwitchVisibility();
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
        
        private IEnumerable<Vector3> GetUnitOffsets()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                yield return _formation.GetUnitOffset(i, _unitSize, _units.Count);
            }
        }

        private void Move(Vector3 joystickDirection)
        {
            var delta = Model.Speed.Value * joystickDirection * Time.deltaTime;
            Destination.transform.position = _world.ClampByWorldBBox(Destination.transform.position + delta);
        }

        private void UpdateUnitsAnimations()
        {
            _units.ForEach(it => { it.MovementController.UpdateAnimation(MoveDirection); });
        }

        private void UpdateSquadRadius()
        {
            var radius = _unitSize;
            var center = Destination.transform.position;
            
            var positionsInCircle = GetUnitOffsets().OrderBy(it => it.magnitude).ToList();
            if (positionsInCircle.Any())
            {
                var furtherPosition = Destination.transform.position + positionsInCircle.Last();
                radius = Vector3.Distance(furtherPosition, center) + _unitSize;
            }
            
            SquadRadius = radius;
        }

        public void RestoreHealth()
        {
            Health.Restore();
        }

        public void AddHealthPercent(int percentFromMax)
        {
            Health.Add(Health.MaxValue.Value * percentFromMax / 100);
        }

        public void CollectAllLoot()
        {
            _lootCollector.CollectAllLoot();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Feofun.Components;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using Survivors.Modifiers;
using Survivors.Squad.Component;
using Survivors.Squad.Formation;
using Survivors.Squad.Model;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using UniRx;
using Unit = Survivors.Units.Unit;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour
    {
        [SerializeField]
        private float _unitSize;

        private readonly IReactiveCollection<Unit> _units = new List<Unit>().ToReactiveCollection();
        private IReadOnlyReactiveProperty<int> _unitCount;
        private readonly ISquadFormation _formation = new CircleFormation();

        private SquadModel _model;
        private IDamageable _damageable;
        private SquadDestination _destination;

        [Inject] private Joystick _joystick;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

        public event Action OnDeath;
        private bool Initialized => _model != null;
        public SquadModel Model => _model;
        public SquadDestination Destination => _destination;
        public IReadOnlyReactiveProperty<int> UnitsCount => _unitCount ??= _units.ObserveCountChanged().ToReactiveProperty();
        public float SquadRadius { get; private set; }
        public bool IsMoving => _joystick.Direction.sqrMagnitude > 0;
        public Vector3 MoveDirection => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        
        public void Awake()
        {
            _destination = gameObject.RequireComponentInChildren<SquadDestination>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            SetUnitPositions();
        }

        public void Init(SquadModel model)
        {
            _model = model;
            _damageable.OnDeath += Kill;
            foreach (var component in GetComponentsInChildren<IInitializable<Squad>>()) {
                component.Init(this);
            }
        }

        private void Kill()
        {
            _damageable.OnDeath -= Kill;
            _units.ForEach(it => it.Kill());
            OnDeath?.Invoke();
            _units.Clear();
        }

        public void AddUnit(Unit unit)
        {
            unit.transform.SetParent(_destination.transform);
            _model.AddUnit(unit.Model);
            _units.Add(unit);
            SetUnitPositions();
            UpdateSquadRadius();
        }
        
        private void AddSquadModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }

        private void AddUnitModifier(IModifier modifier, [CanBeNull] string unitId = null)
        {
            var units = _units.ToList();
            if (unitId != null) {
                units = _units.Where(it => it.Model.Id == unitId).ToList();
            }
            units.ForEach(unit => unit.AddModifier(modifier));
        }

        public void AddModifier(IModifier modifier, ModifierTarget target, [CanBeNull] string unitId = null)
        {
            switch (target) {
                case ModifierTarget.Unit:
                    AddUnitModifier(modifier, unitId);
                    break;
                case ModifierTarget.Squad:
                    AddSquadModifier(modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            _destination.SwitchVisibility();
        }

        private void SetUnitPositions()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++) {
                _units[unitIdx].transform.position = GetUnitPosition(unitIdx);
            }
        }

        private Vector3 GetUnitPosition(int unitIdx)
        {
            return _destination.transform.position + _formation.GetUnitOffset(unitIdx, _unitSize, _units.Count);
        }

        private void Update()
        {
            if (!Initialized) {
                return;
            }
            if (IsMoving) {
                Move(MoveDirection);
            }
            SetUnitPositions();
            UpdateUnitsAnimations();
        }

        private void Move(Vector3 joystickDirection)
        {
            var delta = _model.Speed.Value * joystickDirection * Time.deltaTime;
            _destination.transform.position += delta;
        }

        private void UpdateUnitsAnimations()
        {
            _units.ForEach(it => { it.MovementController.UpdateAnimation(MoveDirection); });
        }
        private Vector3 GetSpawnPosition()
        {
            return _destination.transform.position + _formation.GetSpawnOffset(_unitSize, _units.Count);
        }
        private void OnDestroy()
        {
            _units.Clear();
            _model = null;
        }

        private void UpdateSquadRadius()
        {
            var radius = _unitSize;
            var center = _destination.transform.position;
            foreach (var unit in _units)
            {
                radius = Mathf.Max(radius, Vector3.Distance(unit.transform.position, center) + _unitSize);
            }

            SquadRadius = radius;
        }
    }
}
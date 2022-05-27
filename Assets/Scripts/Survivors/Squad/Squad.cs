using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Feofun.Config;
using Survivors.Session;
using Feofun.Modifiers;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Modifiers;
using Survivors.Squad.Formation;
using Survivors.Squad.Model;
using Survivors.Units;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using UniRx;
using Unit = Survivors.Units.Unit;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour, IWorldCleanUp
    {
        [SerializeField]
        private float _unitSize;

        private readonly IReactiveCollection<Unit> _units = new List<Unit>().ToReactiveCollection();
        private readonly ISquadFormation _formation = new CircleFormation();

        private SquadModel _model;
        private SquadDestination _destination;
        
        [Inject] private Joystick _joystick;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

        private bool Initialized => _model != null;
        public SquadModel Model => _model;
        public SquadDestination Destination => _destination;
        public IReadOnlyReactiveProperty<int> UnitsCount => _units.ObserveCountChanged().ToReactiveProperty();
        public float SquadRadius => _formation.GetMaxSize(_unitSize, _units.Count) / 2;
        public bool IsMoving => _joystick.Direction.sqrMagnitude > 0;
        public Vector3 MoveDirection => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        
        public void Awake()
        {
            _destination = GetComponentInChildren<SquadDestination>();
            SetUnitPositions();
        }

        public void Init(SquadModel model)
        {
            _model = model;

            foreach (var component in GetComponentsInChildren<ISquadInitializable>()) {
                component.Init(this);
            }
        }

        public void AddUnit(Unit unit)
        {
            unit.transform.SetParent(_destination.transform);
            unit.OnDeath += OnUnitDeath;
            _units.Add(unit);
        }

        private void OnUnitDeath(IUnit unit)
        {
            RemoveUnit(unit as Unit);
        }

        public void RemoveUnit(Unit unit)
        {
            Assert.IsTrue(_units.Contains(unit));
            _units.Remove(unit);
            unit.transform.SetParent(null);
            unit.OnDeath -= OnUnitDeath;
        }

        public void AddSquadModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }

        public void AddUnitModifier(IModifier modifier, [CanBeNull]string unitId = null)
        {
            var units = _units.ToList();
            if (unitId != null)
            {
                units = _units.Where(it => it.Model.Id == unitId).ToList();
            }
            units.ForEach(unit => unit.AddModifier(modifier));
        }

        public void AddModifier(IModifier modifier, ModifierTarget target, [CanBeNull]string unitId = null)
        {
            switch (target)
            {
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
            _units.ForEach(it =>
            {
                it.MovementController.PlayAnimation(IsMoving);
                if (it.MovementController.HasTarget) return;
                var targetPos = it.transform.position + MoveDirection;
                it.MovementController.RotateTo(targetPos);
            });
        }

        private Vector3 GetSpawnPosition()
        {
            return _destination.transform.position + _formation.GetSpawnOffset(_unitSize, _units.Count);
        }

        public void OnWorldCleanUp()
        {
            _units.Clear();
            _model = null;
        }
    }
}
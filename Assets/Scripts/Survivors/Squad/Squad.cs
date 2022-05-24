using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Feofun.Config;
using Survivors.Session;
using Feofun.Modifiers;
using LegionMaster.Extension;
using Survivors.Modifiers;
using Survivors.Modifiers.Config;
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
        private float _unitSpeedScale;
        [SerializeField]
        private float _unitSize;

        private readonly List<Unit> _units = new List<Unit>();
        private readonly ISquadFormation _formation = new CircleFormation();

        private SquadDestination _destination;
        private SquadModel _model;

        [Inject] private Joystick _joystick;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject] private ModifierFactory _modifierFactory;

        public SquadModel Model => _model;
        public bool Initialized => _model != null;
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
            unit.transform.SetParent(transform);
            unit.transform.position = GetSpawnPosition();
            unit.MovementController.Init(_model.Speed.Select(speed => speed * _unitSpeedScale).ToReactiveProperty());
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
            unit.OnDeath -= OnUnitDeath;
        }

        public void AddSquadModifier(IModifier modifier)
        {
            Model.AddModifier(modifier);
        }

        public void AddUnitModifier(IModifier modifier)
        {
            _units.ForEach(unit => unit.AddModifier(modifier));
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
        
        /*
        * This is test function. Remove later
        */
        public void AddRandomUnitUpgrade()
        {
            var config = GetRandomUpgradeConfig(ModifierTarget.Unit);
            var modifier = _modifierFactory.Create(config.ModifierConfig);
            Debug.Log($"Adding modifier {config.Id}");
            AddUnitModifier(modifier);
        }
        /*
        * This is test function. Remove later
        */
        private ParameterUpgradeConfig GetRandomUpgradeConfig(ModifierTarget target) =>
                _modifierConfigs.Values.Where(it => it.Target == target).ToList().Random();
        [Button]  
        /*
         * This is test function. Remove later
         */
        public void AddRandomSquadUpgrade()
        {
            var config = GetRandomUpgradeConfig(ModifierTarget.Squad);
            var modifier = _modifierFactory.Create(config.ModifierConfig);
            Debug.Log($"Adding modifier {config.Id}");
            AddSquadModifier(modifier);
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
            if (_joystick.Direction.sqrMagnitude > 0) {
                Move(new Vector3(_joystick.Horizontal, 0, _joystick.Vertical));
            }

            UpdateUnitDestinations();
        }

        private void Move(Vector3 joystickDirection)
        {
            var delta = _model.Speed.Value * joystickDirection * Time.deltaTime;
            _destination.transform.position += delta;
        }

        private void UpdateUnitDestinations()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++) {
                _units[unitIdx].MovementController.MoveTo(GetUnitPosition(unitIdx));
            }
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
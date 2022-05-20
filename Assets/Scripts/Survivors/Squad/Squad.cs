using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Feofun.Config;
using Feofun.Modifiers;
using Feofun.Modifiers.Config;
using LegionMaster.Extension;
using Survivors.Squad.Formation;
using Survivors.Units;
using Survivors.Units.Modifiers;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour
    {
        [SerializeField] private float _unitSpeedScale;
        [SerializeField] private float _unitSize;

        private SquadDestination _destination;
        private readonly List<Unit> _units = new List<Unit>();
        private readonly ISquadFormation _formation = new CircleFormation();

        [Inject] private Joystick _joystick;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private SquadConfig _squadConfig;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject] private ModifierFactory _modifierFactory;

        private void Awake()
        {
            _destination = GetComponentInChildren<SquadDestination>();
            SetUnitPositions();
        }
        
        public void AddUnit(Unit unit)
        {
            unit.transform.SetParent(transform);
            unit.transform.position = GetSpawnPosition();
            unit.MovementController.Init(this, _squadConfig.Params.Speed * _unitSpeedScale);
            _units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }

        public void AddModifier(IModifier modifier)
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

        // This is test function. Remove later
        public void AddRandomUpgrade()
        {
            var modifierId = _modifierConfigs.Keys.Random();
            var modifier = _modifierFactory.Create(_modifierConfigs.Get(modifierId).ModifierConfig);
            Debug.Log($"Adding modifier {modifierId}");
            AddModifier(modifier);
        }

        [Button]
        private void SwitchSquadCenterVisibility()
        {
            _destination.SwitchVisibility();
        }
        
        private void SetUnitPositions()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++)
            {
                _units[unitIdx].transform.position = GetUnitPosition(unitIdx);
            }
        }

        private Vector3 GetUnitPosition(int unitIdx)
        {
            return _destination.transform.position + _formation.GetUnitOffset(unitIdx, _unitSize, _units.Count);
        }

        private void Update()
        {
            if (_joystick.Direction.sqrMagnitude > 0)
            {
                Move(new Vector3(_joystick.Horizontal, 0, _joystick.Vertical));
            }

            UpdateUnitDestinations();
        }

        private void Move(Vector3 joystickDirection)
        {
            var delta = _squadConfig.Params.Speed * joystickDirection * Time.deltaTime;
            _destination.transform.position += delta;
        }

        private void UpdateUnitDestinations()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++)
            {
                _units[unitIdx].MovementController.MoveTo(GetUnitPosition(unitIdx));
            }
        }

        private Vector3 GetSpawnPosition()
        {
            return _destination.transform.position + _formation.GetSpawnOffset(_unitSize, _units.Count);
        }
    }
}
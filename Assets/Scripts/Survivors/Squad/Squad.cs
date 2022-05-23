using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Feofun.Config;
using Survivors.Session;
using Survivors.Squad.Config;
using Survivors.Squad.Formation;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Movement;
using Survivors.Units.Service;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour, IWorldCleanUp
    {
        [SerializeField] private float _unitSpeedScale;
        [SerializeField] private float _unitSize;

        private SquadDestination _destination;
        private readonly List<MovementController> _units = new List<MovementController>();
        private readonly ISquadFormation _formation = new CircleFormation();

        [Inject] private Joystick _joystick;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private SquadConfig _squadConfig;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

        private void Awake()
        {
            _destination = GetComponentInChildren<SquadDestination>();
            SetUnitPositions();
        }
        
        public void AddUnit(MovementController unit)
        {
            unit.transform.SetParent(transform);
            unit.transform.position = GetSpawnPosition();
            unit.Init(this, _squadConfig.Params.Speed * _unitSpeedScale);
            _units.Add(unit);
        }

        public void RemoveUnit(MovementController unit)
        {
            _units.Remove(unit);
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
                _units[unitIdx].MoveTo(GetUnitPosition(unitIdx));
            }
        }

        private Vector3 GetSpawnPosition()
        {
            return _destination.transform.position + _formation.GetSpawnOffset(_unitSize, _units.Count);
        }

        public void OnWorldCleanUp()
        {
            _units.Clear();
        }
    }
}
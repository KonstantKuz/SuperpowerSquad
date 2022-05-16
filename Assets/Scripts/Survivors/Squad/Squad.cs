using System.Collections.Generic;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Player;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using EasyButtons;
using Survivors.Squad.Formation;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _unitSpeedScale;
        [SerializeField] private float _unitSize;

        private SquadDestination _destination;
        private readonly List<MovementController> _units = new List<MovementController>();
        private readonly ISquadFormation _formation = new CircleFormation();

        [Inject] private Joystick _joystick;
        [Inject] private DiContainer _container;

        public void AddUnit(MovementController unit)
        {
            _units.Add(unit);
            unit.SetSpeed(_movementSpeed * _unitSpeedScale);
        }

        [Button]
        /*
         * This functions just tests formation change when new units are added
         */
        private void SpawnUnit()
        {
            Assert.IsTrue(_units.Count > 0);
            var newUnit = _container.InstantiatePrefabForComponent<MovementController>(_units[0]);
            newUnit.transform.SetParent(transform);
            newUnit.transform.position = GetSpawnPosition();
            AddUnit(newUnit);
        }

        [Button]
        private void SwitchSquadCenterVisibility()
        {
            _destination.SwitchVisibility();
        }

        private void Awake()
        {
            _destination = GetComponentInChildren<SquadDestination>();
            GetComponentsInChildren<MovementController>().ForEach(AddUnit);
            SetUnitPositions();
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
            var delta = _movementSpeed * joystickDirection * Time.deltaTime;
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
    }
}
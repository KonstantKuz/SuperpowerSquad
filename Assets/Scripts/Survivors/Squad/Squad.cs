﻿using System.Collections.Generic;
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

        private readonly List<MovementController> _units = new List<MovementController>();
        private readonly ISquadFormation _formation = new CircleFormation();
        
        [Inject] 
        private Joystick _joystick;
        [Inject] 
        private DiContainer _container;

        public void AddUnit(MovementController unit)
        {
            AddUnitInner(unit);
            UpdateUnitDestinations();
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
            newUnit.transform.position = GetSpawnPos();
            AddUnit(newUnit);
        }

        [Button]
        private void SwitchSquadCenterVisibility()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = !meshRenderer.enabled;
        }

        private void AddUnitInner(MovementController unit)
        {
            _units.Add(unit);
            unit.SetSpeed(_movementSpeed * _unitSpeedScale);
        }

        private void Awake()
        {
            GetComponentsInChildren<MovementController>().ForEach(AddUnitInner);
            SetUnitPositions();
        }

        private void SetUnitPositions()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++)
            {
                _units[unitIdx].transform.position = transform.position + _formation.GetUnitOffset(unitIdx, _unitSize, _units.Count);
            }
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
            transform.position += delta;
            
            //keep unit positions same as before
            foreach (var unit in _units)
            {
                unit.transform.position -= delta;
            }
        }

        private void UpdateUnitDestinations()
        {
            for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++)
            {
                _units[unitIdx].MoveTo(transform.position + _formation.GetUnitOffset(unitIdx, _unitSize, _units.Count));
            }
        }

        private Vector3 GetSpawnPos()
        {
            return transform.position + _formation.GetSpawnOffset(_unitSize, _units.Count);
        }
    }
}
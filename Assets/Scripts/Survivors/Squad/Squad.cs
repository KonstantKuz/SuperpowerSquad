using System.Collections.Generic;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Player;
using UnityEngine;
using Zenject;

namespace Survivors.Squad
{
    public class Squad : MonoBehaviour
    {
        [SerializeField] private float _unitSize;

        private readonly List<MovementController> _units = new List<MovementController>();
        
        [Inject] 
        private Joystick _joystick;

        public void AddUnit(MovementController unit)
        {
            _units.Add(unit);
            //TODO: change unit positions...
        }

        private void Awake()
        {
            GetComponentsInChildren<MovementController>().ForEach(AddUnit);    
        }

        private void Update()
        {
            if (_joystick.Direction.sqrMagnitude > 0)
            {
                var destination = transform.position + new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
                for (int unitIdx = 0; unitIdx < _units.Count; unitIdx++)
                {
                    _units[unitIdx].MoveTo(destination + GetUnitOffset(unitIdx));
                }
            }
            else
            {
                _units.ForEach(unit => unit.Stop());
            }

            UpdateSquadPosition();
        }

        private void UpdateSquadPosition()
        {
            var centerPosition = GetCenter();
            var delta = centerPosition - transform.position;
            transform.position = centerPosition;
            foreach (var unit in _units)
            {
                unit.transform.position -= delta;
            }
        }

        private Vector3 GetUnitOffset(int unitIdx)
        {
            var radius = _units.Count == 1 ? 0 : _units.Count * _unitSize / Mathf.PI / 2;
            var angle = 360 * unitIdx / _units.Count;
            return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * radius;
        }

        private Vector3 GetCenter()
        {
            var center = new Vector3(0, transform.position.y, 0);
            foreach (var unit in _units)
            {
                var unitPos = unit.transform.position;
                center += new Vector3(unitPos.x, 0, unitPos.z);
            }

            return new Vector3(center.x / _units.Count, center.y, center.z / _units.Count); 
        }
    }
}
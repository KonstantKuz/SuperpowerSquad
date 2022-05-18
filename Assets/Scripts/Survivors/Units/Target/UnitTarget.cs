using System;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Target
{
    public class UnitTarget : MonoBehaviour, ITarget
    {
        private static int _idCount;
        
        [SerializeField]
        private UnitType _unitType;
        [SerializeField] 
        private Transform _centerTarget;

        [Inject] 
        private TargetService _targetService;

        public Action OnTargetInvalid { get; set; }
        public Transform Root => transform;
        public bool IsAlive { get; private set; } = true;
        public Transform Center => _centerTarget;

        public UnitType UnitType
        {
            get => _unitType;
            set
            {
                _targetService.Remove(this);
                _unitType = value;
                _targetService.Add(this);
            }
        }

        public string TargetId
        {
            get;
            private set;
        }

        private void Awake()
        {
            
            TargetId = $"{_unitType.ToString()}#{_idCount++}";
            _targetService.Add(this);
        }

        public void OnDeath()
        {
            if (!IsAlive) return;
            IsAlive = false;
            _targetService.Remove(this);            
            OnTargetInvalid?.Invoke();
        }

        private void OnDestroy()
        {
            if (!IsAlive) return;
            _targetService.Remove(this);
            OnTargetInvalid?.Invoke();
        }
    }
}
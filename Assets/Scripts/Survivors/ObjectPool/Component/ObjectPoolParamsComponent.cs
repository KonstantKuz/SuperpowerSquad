using System;
using UnityEngine;

namespace Survivors.ObjectPool.Component
{
    public class ObjectPoolParamsComponent : MonoBehaviour
    {
        [SerializeField]
        private int _initialCapacity = 300;
        [SerializeField]
        private bool _detectInitialCapacityShortage = true;
        [SerializeField]
        private int _maxCapacity = 3000;
        [SerializeField]
        private int _sizeIncrementStep = 1;
        
        [SerializeField]
        private bool _preparePoolOnInitScene;
        [SerializeField]
        private MonoBehaviour _poolType;
        
        public bool PreparePoolOnInitScene => _preparePoolOnInitScene;
        public Type PoolType => _poolType.GetType();
        
        public ObjectPoolParams GetPoolParams()
        {
            return new ObjectPoolParams() {
                    InitialCapacity = _initialCapacity,
                    DetectInitialCapacityShortage = _detectInitialCapacityShortage,
                    MaxCapacity = _maxCapacity,
                    SizeIncrementStep = _sizeIncrementStep,
            };
        }
    }
}
using UnityEngine;

namespace Survivors.ObjectPool.Component
{
    public class ObjectPoolParamsComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _isCollectionCheck = true;
        [SerializeField]
        private int _initialCapacity = 100;
        [SerializeField]
        private int _maxSize = 2000;
        [SerializeField]
        private ObjectCreateMode _objectCreateMode = ObjectCreateMode.Group;
        [SerializeField]
        private bool _disposeActive = true;

        
        public ObjectPoolParams GetPoolParams()
        {
            return new ObjectPoolParams() {
                    IsCollectionCheck = _isCollectionCheck,
                    InitialCapacity = _initialCapacity,
                    MaxCapacity = _maxSize,
                    ObjectCreateMode = _objectCreateMode,
                    DisposeActive = _disposeActive,
            };
        }
    }
}
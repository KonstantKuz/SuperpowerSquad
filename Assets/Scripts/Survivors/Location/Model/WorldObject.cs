using JetBrains.Annotations;
using Survivors.ObjectPool;
using Survivors.ObjectPool.Component;
using UnityEngine;

namespace Survivors.Location.Model
{
    public class WorldObject : MonoBehaviour
    {
        [SerializeField]
        private string _objectId;
        
        [SerializeField]
        private ObjectType _objectType;      
       
        [SerializeField]
        private bool _usePool;

        [SerializeField]
        private ObjectPoolParamsComponent _objectPoolParams;
        
        public void Reset()
        {
            ObjectId = gameObject.name;
        }
        public string ObjectId
        {
            get => _objectId;
            private set { _objectId = value; }
        }
        public GameObject GameObject => gameObject;
        public ObjectType ObjectType => _objectType;

        [CanBeNull]
        public ObjectPoolParamsComponent ObjectPoolParams => _objectPoolParams;

    }
}
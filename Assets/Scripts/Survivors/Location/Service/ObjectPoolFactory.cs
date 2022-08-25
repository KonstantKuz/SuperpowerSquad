using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.ObjectPool.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location.Service
{
    public class ObjectPoolFactory : IWorldObjectFactory
    {
        [Inject]
        private PoolManager _poolManager;
        [Inject]
        private ObjectResourceService _objectResourceService;
        public T CreateObject<T>(string objectId, Transform container = null) where T : MonoBehaviour
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return GetPoolObject<T>(prefab.GameObject, container).RequireComponent<T>();
        }

        public void DestroyObject<T>(GameObject item) where T : MonoBehaviour
        {
            _poolManager.Release<T>(item);
        }

        public void DestroyAllObjects()
        {
            _poolManager.ReleaseAllActive();
        }
        private GameObject GetPoolObject<T>(GameObject prefab, [CanBeNull] Transform container = null) where T : MonoBehaviour
        {
            var poolObject = _poolManager.Get<T>(prefab);
            if (container != null) {
                poolObject.transform.SetParent(container);
            }
            return poolObject;
        }
    }
}
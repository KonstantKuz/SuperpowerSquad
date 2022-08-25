using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.ObjectPool.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location.ObjectFactory
{
    public class ObjectPoolFactory : IObjectFactory
    {
        [Inject] private PoolManager _poolManager;
        [Inject] private ObjectResourceService _objectResourceService;

        public T Create<T>(string objectId, Transform container = null) where T : MonoBehaviour
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(prefab.GameObject, container);
        }

        public T Create<T>(GameObject prefab, Transform container = null) where T : MonoBehaviour
        {
            return GetPoolObject<T>(prefab, container).RequireComponent<T>();
        }

        public void Destroy<T>(GameObject item) where T : MonoBehaviour
        {
            _poolManager.Release<T>(item);
        }

        public void DestroyAllObjects()
        {
            _poolManager.ReleaseAllActive();
        }

        private GameObject GetPoolObject<T>(GameObject prefab, [CanBeNull] Transform container = null)
            where T : MonoBehaviour
        {
            var poolObject = _poolManager.Get<T>(prefab);
            if (container != null)
            {
                poolObject.transform.SetParent(container);
            }

            return poolObject;
        }
    }
}
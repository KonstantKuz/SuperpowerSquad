using System.Linq;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using Survivors.ObjectPool.Component;
using Survivors.ObjectPool.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location.ObjectFactory.Factories
{
    public class ObjectPoolFactory : IObjectFactory
    {
        [Inject] private PoolManager _poolManager;
        [Inject] private ObjectResourceService _objectResourceService;

        public void Prepare()
        {
            _objectResourceService.GetAllPrefabs()
                                  .Select(it => it.GetComponent<ObjectPoolParamsComponent>())
                                  .Where(it => it != null && it.PreparePoolOnInitScene)
                                  .ForEach(it => _poolManager.Prepare(it.PoolType, it.gameObject, it.GetPoolParams()));
        
        }

        public T Create<T>(string objectId, Transform container = null) where T : MonoBehaviour
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(prefab.GameObject, container);
        }

        public T Create<T>(GameObject prefab, Transform container = null) where T : MonoBehaviour
        {
            return GetPoolObject<T>(prefab, container).RequireComponent<T>();
        }

        public void Destroy<T>(GameObject instance) where T : MonoBehaviour
        {
            _poolManager.Release<T>(instance);
        }

        public void DestroyAllObjects()
        {
            _poolManager.ReleaseAllActive();
        }

        private GameObject GetPoolObject<T>(GameObject prefab, [CanBeNull] Transform container = null) 
                where T : MonoBehaviour
        {
            var poolObject = _poolManager.Get<T>(prefab);
            if (container != null) {
                poolObject.transform.SetParent(container);
            }

            return poolObject;
        }
    }
}
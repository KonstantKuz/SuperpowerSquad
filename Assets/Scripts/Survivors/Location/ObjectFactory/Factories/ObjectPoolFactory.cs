using System;
using Feofun.Extension;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.Location.Service;
using Survivors.ObjectPool;
using Survivors.ObjectPool.Component;
using Survivors.ObjectPool.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location.ObjectFactory.Factories
{
    public class ObjectPoolFactory : IObjectFactory
    {
        [Inject]
        private PoolManager _poolManager;
        [Inject]
        private ObjectResourceService _objectResourceService;

        public T Create<T>(string objectId, Transform container = null)
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(objectId, prefab.GameObject, container);
        }

        public T Create<T>(string objectId, GameObject prefab, Transform container = null)
        {
            return GetPoolObject(objectId, prefab, container).RequireComponent<T>();
        }

        public void Destroy(GameObject instance)
        {
            try {
                _poolManager.Release(instance);
            } catch (Exception exception) {
                this.Logger().Error($"Object release exception to the pool, obj:= {instance.name}", exception);
            }
        }

        public void DestroyAllObjects()
        {
            try {
                _poolManager.ReleaseAllActive();
            } catch (Exception exception) {
                this.Logger().Error("Objects release exception to the pool", exception);
            }
         
        }

        private GameObject GetPoolObject(string objectId, GameObject prefab, [CanBeNull] Transform container = null)
        {
            var poolObject = _poolManager.Get(objectId, prefab, TryGetPoolParams(objectId, prefab));
            if (container != null) {
                poolObject.transform.SetParent(container);
            }
            return poolObject;
        }

        [CanBeNull]
        private ObjectPoolParams TryGetPoolParams(string objectId, GameObject prefab)
        {
            if (_poolManager.HasPool(objectId)) {
                return null;
            }
            var poolParamsComponent = prefab.GetComponent<ObjectPoolParamsComponent>();
            return poolParamsComponent == null ? null : poolParamsComponent.GetPoolParams();
        }
    }
}
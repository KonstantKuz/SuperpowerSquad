using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.ObjectPool.Wrapper;
using UnityEngine;

namespace Survivors.ObjectPool.Service
{
    public class PoolManager : IDisposable
    {
        private static readonly ObjectPoolParams DefaultPoolParams = new ObjectPoolParams {
                InitialCapacity = 100,
                MaxCapacity = 2000,
                ObjectCreateMode = ObjectCreateMode.Group,
                DisposeActive = true,
        };

        private readonly Dictionary<Type, IObjectPool<GameObject>> _pools = new Dictionary<Type, IObjectPool<GameObject>>();
        
        private readonly IObjectPoolWrapper _objectPoolWrapper;
        
        public PoolManager(IObjectPoolWrapper objectPoolWrapper)
        {
            _objectPoolWrapper = objectPoolWrapper;
        }
        public GameObject Get<T>(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                _pools[type] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams ?? DefaultPoolParams);
            }
            return _pools[type].Get();
        }

        public void Release<T>(GameObject instance) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                throw new NullReferenceException($"ObjectPool is null by object type:= {type}");
            }
            _pools[type].Release(instance);
        }

        public void ReleaseAllActive()
        {
            foreach (var pool in _pools.Values) {
                pool.ReleaseAllActive();
            }
        }
        public void Dispose()
        {
            foreach (var pool in _pools.Values) {
                pool.Dispose();
            }
        }

    }
}
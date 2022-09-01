using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.ObjectPool.Wrapper;
using UnityEngine;

namespace Survivors.ObjectPool.Service
{
    public class PoolManager : IDisposable
    {
        
        private readonly Dictionary<Type, IObjectPool<GameObject>> _pools = new Dictionary<Type, IObjectPool<GameObject>>();
        
        private readonly IObjectPoolWrapper _objectPoolWrapper;
        
        public PoolManager(IObjectPoolWrapper objectPoolWrapper)
        {
            _objectPoolWrapper = objectPoolWrapper;
        }
        public void Prepare<T>(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            var typePool = typeof(T);
            Prepare(typePool, prefab, poolParams);
        }   
        public void Prepare(Type typePool, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (_pools.ContainsKey(typePool)) {
                throw new ArgumentException($"Object pool already prepared by object type, type:= {typePool}");
            }
            _pools[typePool] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
        }
        public bool HasPool<T>() => _pools.ContainsKey(typeof(T));

        public GameObject Get<T>(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                _pools[type] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
            }
            return _pools[type].Get();
        }

        public void Release<T>(GameObject instance)
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
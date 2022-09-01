using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.ObjectPool.Wrapper;
using UnityEngine;

namespace Survivors.ObjectPool.Service
{
    public class PoolManager : IDisposable
    {
        private readonly Dictionary<string, IObjectPool<GameObject>> _pools = new Dictionary<string, IObjectPool<GameObject>>();
        
        private readonly IObjectPoolWrapper _objectPoolWrapper;
        
        public PoolManager(IObjectPoolWrapper objectPoolWrapper)
        {
            _objectPoolWrapper = objectPoolWrapper;
        }
        public void Prepare(string id, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (_pools.ContainsKey(id)) {
                throw new ArgumentException($"Object pool already prepared by pool id, id:= {id}");
            }
            _pools[id] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
        }
        public bool HasPool(string id) => _pools.ContainsKey(id);

        public GameObject Get(string id, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (!_pools.ContainsKey(id)) {
                _pools[id] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
            }
            return _pools[id].Get();
        }

        public void Release(string id, GameObject instance)
        {
            if (!_pools.ContainsKey(id)) {
                throw new NullReferenceException($"ObjectPool is null by pool id:= {id}");
            }
            _pools[id].Release(instance);
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
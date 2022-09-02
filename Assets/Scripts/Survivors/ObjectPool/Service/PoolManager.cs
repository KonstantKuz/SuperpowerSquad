using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.ObjectPool.Component;
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
        public void Prepare(string poolId, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (_pools.ContainsKey(poolId)) {
                throw new ArgumentException($"Object pool already prepared by pool id, id:= {poolId}");
            }
            _pools[poolId] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
        }
        public bool HasPool(string poolId) => _pools.ContainsKey(poolId);

        public GameObject Get(string poolId, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (!_pools.ContainsKey(poolId)) {
                _pools[poolId] = _objectPoolWrapper.BuildObjectPool(prefab, poolParams);
            }
            var item = _pools[poolId].Get();
            if (!item.TryGetComponent(out ObjectPoolIdentifier poolIdentifier)) {
                item.AddComponent<ObjectPoolIdentifier>().PoolId = poolId;
            }
            return item;
        }

        public void Release(GameObject instance)
        {
            if (!instance.TryGetComponent(out ObjectPoolIdentifier poolIdentifier)) {
                throw new NullReferenceException($"Error releasing gameObject to the pool, instance does't contain ObjectPoolIdentifier, gameObject name:= {instance.name}");
            }
            var poolId = poolIdentifier.PoolId;
            if (!_pools.ContainsKey(poolId)) {
                throw new NullReferenceException($"ObjectPool is null by pool id:= {poolId}");
            }
            _pools[poolId].Release(instance);
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
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Survivors.ObjectPool.Service
{
    public class PoolManager : MonoBehaviour
    {
        private static readonly ObjectPoolParams PoolParams = new ObjectPoolParams {
                IsCollectionCheck = true,
                InitialCapacity = 100,
                MaxSize = 2000,
                ObjectCreateMode = ObjectCreateMode.Group,
                DisposeActive = true,
        };

        private readonly Dictionary<Type, IObjectPool<GameObject>> _pools = new Dictionary<Type, IObjectPool<GameObject>>();

        [SerializeField]
        private Transform _poolRoot;

        [Inject]
        private DiContainer _container;
        
        public GameObject Get<T>(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                _pools[type] = BuildObjectPool(prefab, poolParams);
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

        private ObjectPool<GameObject> BuildObjectPool(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        { 
            return new ObjectPool<GameObject>(() => OnCreateObject(prefab), OnGetFromPool, OnReleaseToPool, OnDestroyObject, poolParams ?? PoolParams);
        }

        private GameObject OnCreateObject(GameObject prefab)
        {
            var createdGameObject = _container.InstantiatePrefab(prefab, _poolRoot);
            createdGameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            return createdGameObject;
        }

        private void OnGetFromPool(GameObject instance)
        {
            instance.transform.SetParent(_poolRoot);
            instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            instance.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(GameObject instance)
        {
            instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject(GameObject instance)
        {
            Destroy(instance.gameObject);
        }
    }
}
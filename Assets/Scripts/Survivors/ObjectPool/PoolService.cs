using System;
using System.Collections.Generic;
using Survivors.Extension;
using UnityEngine;
using Zenject;

namespace Survivors.ObjectPool
{
    public class PoolService : MonoBehaviour
    {
        private readonly Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();

        [SerializeField]
        private Transform _poolRoot;

        [Inject]
        private DiContainer _container;

        public T Get<T>(GameObject prefab)
                where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                _pools[type] = BuildObjectPool<T>(prefab);
            }
            return _pools[type].Get<T>();
        }

        public void Release<T>(T element)
                where T : MonoBehaviour
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type)) {
                throw new NullReferenceException("ObjectPool is null");
            }
            _pools[type].Release(element);
        }

        public void ReleaseAllActive()
        {
            foreach (var pool in _pools.Values) {
                pool.ReleaseAllActive();
            }
        }

        private IObjectPool BuildObjectPool<T>(GameObject prefab)
                where T : MonoBehaviour
        {
            var poolContainer = new ObjectPoolAdapter();
            poolContainer.Create(new ObjectPool<T>(() => OnCreateObject<T>(prefab), OnGetFromPool, OnReleaseToPool, OnDestroyObject, true, 
                                                   200,
                                                   10000, ObjectCreateMode.Group));
            return poolContainer;
        }

        private T OnCreateObject<T>(GameObject prefab)
                where T : MonoBehaviour
        {
            var createdGameObject = _container.InstantiatePrefab(prefab, _poolRoot);
            createdGameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            createdGameObject.gameObject.SetActive(false);

            return createdGameObject.RequireComponent<T>();
        }

        private void OnGetFromPool<T>(T instance)
                where T : MonoBehaviour
        {
            instance.transform.SetParent(_poolRoot);
            instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            instance.gameObject.SetActive(true);
        }

        private void OnReleaseToPool<T>(T instance)
                where T : MonoBehaviour
        {
            instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject<T>(T instance)
                where T : MonoBehaviour
        {
            Destroy(instance.gameObject);
        }
    }
}
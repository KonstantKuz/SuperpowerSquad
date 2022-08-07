using System;
using System.Collections.Generic;
using Survivors.Extension;
using UnityEngine;
using Zenject;

namespace Survivors.Location.Service
{
    public class PoolService : MonoBehaviour
    {
        
        private readonly Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();

        [SerializeField]
        private Transform _poolContainer;

        [Inject] private DiContainer _container;
        

        public T Get<T>(GameObject prefab) where T : MonoBehaviour
        {
            var type = typeof(T);
            
            if (!_pools.ContainsKey(type))
            {
                _pools[type] = BuildPoolContainer<T>(prefab);
            }
            return _pools[type].Get<T>();
        }
        public void Release<T>(T element) where T : MonoBehaviour
        {
         
            var type = typeof(T);
            
            if (!_pools.ContainsKey(type))
            {
                throw new NullReferenceException("IObjectPoolContainer is null");
            }
            _pools[type].Release(element);
        }
        public void ReleaseAllActive()
        {
            foreach (var pool in _pools.Values)
            {
                pool.ReleaseAllActive();
            }
        }

        private IObjectPool BuildPoolContainer<T>(GameObject prefab) where T: MonoBehaviour
        {
            var poolContainer = new ObjectPoolProxy();
            poolContainer.Create(() => CreatePooledObject<T>(prefab),OnTakeFromPool,OnReturnToPool, OnDestroyObject, 
                false, 
                10, 
                10000, ObjectCreateMode.Bank);
            return poolContainer;
        }
        private T CreatePooledObject<T>(GameObject prefab) where T: MonoBehaviour
        {
            var createdGameObject = _container.InstantiatePrefab(prefab, _poolContainer);
            createdGameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            createdGameObject.gameObject.SetActive(false);

            return createdGameObject.RequireComponent<T>();
        }
        
        private void OnTakeFromPool<T>(T instance) where T: MonoBehaviour
        {
            instance.transform.SetParent(_poolContainer);
            instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            instance.gameObject.SetActive(true);
        }

        private void OnReturnToPool<T>(T instance) where T: MonoBehaviour
        {
            instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject<T>(T instance) where T : MonoBehaviour
        {
            Destroy(instance.gameObject);
        }

    }
}
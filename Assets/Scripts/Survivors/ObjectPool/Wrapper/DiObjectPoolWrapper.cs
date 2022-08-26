using System;
using UnityEngine;
using Zenject;

namespace Survivors.ObjectPool.Wrapper
{
    public class DiObjectPoolWrapper : MonoBehaviour, IObjectPoolWrapper
    {
        [Inject]
        private DiContainer _container;
        [SerializeField]
        private Transform _poolRoot;
        
        public IObjectPool<GameObject> BuildObjectPool(GameObject prefab, ObjectPoolParams poolParams)
        {
            return new ObjectPool<GameObject>(() => OnCreateObject(prefab), OnGetFromPool, OnReleaseToPool, OnDestroyObject, poolParams);
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
            instance.transform.SetParent(_poolRoot);
            instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject(GameObject instance)
        {
            Destroy(instance.gameObject);
        }
    }
}
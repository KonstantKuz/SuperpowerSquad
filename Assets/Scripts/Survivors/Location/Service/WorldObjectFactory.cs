using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.ObjectPool;
using Survivors.ObjectPool.Service;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

namespace Survivors.Location.Service
{
    public class WorldObjectFactory : MonoBehaviour, IWorldScope, IWorldObjectFactory 
    {
        private const string OBJECT_PREFABS_PATH_ROOT = "Content/";

        private readonly Dictionary<string, WorldObject> _prefabs = new Dictionary<string, WorldObject>();

        private readonly HashSet<GameObject> _createdObjects = new HashSet<GameObject>();
        
        private CompositeDisposable _disposable;

        [Inject]
        private PoolManager _poolManager;

        [Inject]
        private World _world;
        [Inject]
        private DiContainer _container;

        public void Init()
        {
            _disposable = new CompositeDisposable();
            LoadWorldObjects();
        }

        private void LoadWorldObjects()
        {
            var worldObjects = Resources.LoadAll<WorldObject>(OBJECT_PREFABS_PATH_ROOT);
            foreach (var worldObject in worldObjects) {
                _prefabs.Add(worldObject.ObjectId, worldObject);
            }
        }
        public T CreateObject<T>(string objectId, [CanBeNull] Transform container = null) where T : MonoBehaviour
        {
            if (!_prefabs.ContainsKey(objectId)) {
                throw new KeyNotFoundException($"No prefab with objectId {objectId} found");
            }
            var prefab = _prefabs[objectId];
            var instance = prefab.UsePool ? GetPoolObject<T>(prefab) : CreateObject(prefab.GameObject, container);
            return instance.RequireComponent<T>();
        }
        public void DestroyObject<T>(GameObject item) where T : MonoBehaviour
        {
            var worldObject = item.gameObject.RequireComponent<WorldObject>();
            if (worldObject.UsePool) {
                ReleasePoolObject<T>(item);
                return;
            }
            Destroy(item.gameObject);
        }
        public GameObject CreateObject(GameObject prefab, [CanBeNull] Transform container = null)
        {
            var parentContainer = container == null ? _world.Spawn.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => RemoveObject(createdGameObject)).AddTo(_disposable);
            return createdGameObject;
        }
        
        public GameObject GetPoolObject<T>(WorldObject prefab) where T : MonoBehaviour
        {
            var poolObj = _poolManager.Get<T>(prefab.gameObject, prefab.ObjectPoolParams?.GetPoolParams());
            _createdObjects.Add(poolObj);
            poolObj.OnDisableAsObservable().Subscribe((o) => RemoveObject(poolObj.gameObject)).AddTo(_disposable);
            poolObj.OnDestroyAsObservable().Subscribe((o) => RemoveObject(poolObj.gameObject)).AddTo(_disposable);
            return poolObj;
        }
        public void ReleasePoolObject<T>(GameObject instance) where T : MonoBehaviour
        {
            _poolManager.Release<T>(instance);
        }
        
        public List<T> GetObjectComponents<T>()
        {
            return _createdObjects.Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }
        
        private void RemoveObject(GameObject obj) => _createdObjects.Remove(obj);
        
        private void DestroyAllObjects()
        {
            _poolManager.ReleaseAllActive();
            
            foreach (var gameObject in _createdObjects) {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public void OnWorldSetup()
        {
           
        }

        public void OnWorldCleanUp()
        {
            DestroyAllObjects();
        }
    }

    public interface IWorldObjectFactory
    { 
        T CreateObject<T>(string objectId, [CanBeNull] Transform container = null) where T : MonoBehaviour; 
        void DestroyObject<T>(GameObject item) where T : MonoBehaviour;
    }
}
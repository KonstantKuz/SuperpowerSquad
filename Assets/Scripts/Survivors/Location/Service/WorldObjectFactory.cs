using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.ObjectPool;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

namespace Survivors.Location.Service
{
    public class WorldObjectFactory : MonoBehaviour, IWorldScope
    {
        private const string OBJECT_PREFABS_PATH_ROOT = "Content/";

        private readonly Dictionary<string, WorldObject> _prefabs = new Dictionary<string, WorldObject>();

        private readonly HashSet<GameObject> _createdObjects = new HashSet<GameObject>();
        
        private CompositeDisposable _disposable;

        [Inject]
        private PoolService _poolService;

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
            return prefab.UsePool ? GetPoolObject<T>(prefab.GameObject) : CreateObject(prefab.GameObject, container).RequireComponent<T>();
        }
        public void DestroyObject<T>(T item) where T : MonoBehaviour
        {
            var worldObject = item.gameObject.RequireComponent<WorldObject>();
            if (worldObject.UsePool) {
                ReleasePoolObject(item);
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
        
        public T GetPoolObject<T>(GameObject prefab) where T : MonoBehaviour
        {
            var obj = _poolService.Get<T>(prefab);
            _createdObjects.Add(obj.gameObject);
            obj.gameObject.OnDisableAsObservable().Subscribe((o) => RemoveObject(obj.gameObject)).AddTo(_disposable);
            return obj;
        }
        public void ReleasePoolObject<T>(T item) where T : MonoBehaviour
        {
            _poolService.Release(item);
        }
        
        public List<T> GetObjectComponents<T>()
        {
            return _createdObjects.Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }
        
        private void RemoveObject(GameObject obj) => _createdObjects.Remove(obj);
        
        private void DestroyAllObjects()
        {
            _poolService.ReleaseAllActive();
            
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
}
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

        private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

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
                _prefabs.Add(worldObject.ObjectId, worldObject.GameObject);
            }
        }
        
        public T CreateObject<T>(string objectId, [CanBeNull] Transform container = null, bool usePool = false) where T : MonoBehaviour
        {
            if (!_prefabs.ContainsKey(objectId)) {
                throw new KeyNotFoundException($"No prefab with objectId {objectId} found");
            }
            var prefab = _prefabs[objectId];
            return usePool ? CreateMyPoolingGameObject<T>(prefab) : CreateObject(prefab, container).RequireComponent<T>();
        }

        public GameObject CreateObject(GameObject prefab, [CanBeNull] Transform container = null)
        {
            
            var parentContainer = container == null ? _world.Spawn.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => OnDestroyObject(createdGameObject)).AddTo(_disposable);
            return createdGameObject;
        }

/*        public GameObject CreatePoolingGameObject(GameObject prefab)
        {
            if (!PoolManager.IsWarmPool(prefab)) {
                PoolManager.WarmPool(prefab, 500);
            }
            var poolingGameObjet = PoolManager.SpawnObject(prefab);
            _contaziner.InjectGameObject(poolingGameObjet);
            return poolingGameObjet;
        } */
        public T CreateMyPoolingGameObject<T>(GameObject prefab) where T : MonoBehaviour
        {
            var obj = _poolService.Get<T>(prefab);
            _createdObjects.Add(obj.gameObject);
            obj.gameObject.OnDisableAsObservable().Subscribe((o) => OnDestroyObject(obj.gameObject)).AddTo(_disposable);
            return obj;
        }

        public void ReleaseObject<T>(T item) where T : MonoBehaviour
        {
            _poolService.Release(item);
        }

        private void OnDestroyObject(GameObject obj)
        {
            _createdObjects.Remove(obj);
        }

        public List<T> GetObjectComponents<T>()
        {
            return _createdObjects.Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }

        public void DestroyAllObjects()
        {
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
            _poolService.ReleaseAllActive();
        }
    }
}
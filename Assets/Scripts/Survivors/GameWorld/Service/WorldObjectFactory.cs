using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.GameWorld.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Survivors.GameWorld.Service
{
    public class WorldObjectFactory : MonoBehaviour
    {
        private const string OBJECT_PREFABS_PATH_ROOT = "Content/World/";

        private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

        private readonly List<GameObject> _createdObjects = new List<GameObject>();

        [Inject]
        private World _world;
        [Inject]
        private DiContainer _container;
        
        private CompositeDisposable _disposable;

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

        public GameObject CreateObject(string objectId, [CanBeNull] GameObject container = null)
        {
            if (!_prefabs.ContainsKey(objectId)) {
                throw new KeyNotFoundException($"No prefab with objectId {objectId} found");
            }
            var prefab = _prefabs[objectId];
            return CreateObject(prefab, container);
        }

        public GameObject CreateObject(GameObject prefab, [CanBeNull] GameObject container = null)
        {
            var parentContainer = container == null ? _world.Spawn.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => OnDestroyObject(createdGameObject)).AddTo(_disposable);
            return createdGameObject;
        }

        private void OnDestroyObject(GameObject obj)
        {
            _createdObjects.Remove(obj);
        }

        public void Term()
        {
            foreach (var gameObject in _createdObjects) {
                Destroy(gameObject);
            }
            Dispose();
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;

        }
        private void OnDestroy()
        {
            Dispose();
        }
        public List<T> GetObjectComponents<T>()
        {
            return _createdObjects.Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }
    }
}
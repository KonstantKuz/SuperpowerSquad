﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.Location.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Survivors.Location.Service
{
    public class LocationObjectFactory : MonoBehaviour
    {
        private const string OBJECT_PREFABS_PATH_ROOT = "Content/Location/";

        private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

        private readonly List<GameObject> _createdObjects = new List<GameObject>();

        [Inject]
        private Location _location;
        [Inject]
        private DiContainer _container;

        public LocationObjectFactory()
        {
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
            var parentContainer = container == null ? _location.SpawnContainer.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => OnDestroyObject(createdGameObject));
            return createdGameObject;
        }

        private void OnDestroyObject(GameObject obj)
        {
            _createdObjects.Remove(obj);
        }

        public void DestroyAllObjects()
        {
            foreach (var gameObject in _createdObjects) {
                Destroy(gameObject);
            }
        }

        public List<T> GetObjectComponents<T>()
        {
            return _createdObjects.Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }
    }
}
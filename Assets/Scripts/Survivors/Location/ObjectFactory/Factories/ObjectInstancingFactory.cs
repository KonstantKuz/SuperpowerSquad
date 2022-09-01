using System.Collections.Generic;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using Survivors.Location.Service;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Survivors.Location.ObjectFactory.Factories
{
    public class ObjectInstancingFactory : MonoBehaviour, IObjectFactory 
    {
        private readonly HashSet<GameObject> _createdObjects = new HashSet<GameObject>();
        
        private CompositeDisposable _disposable;
        
        [Inject]
        private World _world;     
        [Inject]
        private ObjectResourceService _objectResourceService;
        [Inject]
        private DiContainer _container;

        private CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();

        public T Create<T>(string objectId, Transform container = null)
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(prefab.GameObject, container);
        }
        public T Create<T>(GameObject prefab, Transform container = null)
        {
            return CreateObject(prefab, container).RequireComponent<T>();
        }
        private GameObject CreateObject(GameObject prefab, [CanBeNull] Transform container = null)
        {
            var parentContainer = container == null ? _world.Spawn.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => RemoveObject(createdGameObject)).AddTo(Disposable);
            return createdGameObject;
        }
        public void DestroyAllObjects()
        {
            _createdObjects.ForEach(Destroy);
            Dispose();
            _createdObjects.Clear();
        }
        private void RemoveObject(GameObject obj) => _createdObjects.Remove(obj);
        
        private void OnDestroy()
        {
            Dispose();
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
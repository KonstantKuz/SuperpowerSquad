using System.Collections.Generic;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Survivors.Location.ObjectFactory
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
        
        public T Create<T>(string objectId, Transform container = null) where T : MonoBehaviour
        {
            TryCreateDisposable();
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(prefab.GameObject, container);
        }
        public T Create<T>(GameObject prefab, [CanBeNull] Transform container = null) where T : MonoBehaviour
        {
            return CreateObject(prefab, container).RequireComponent<T>();
        }
        public void Destroy<T>(GameObject item) where T : MonoBehaviour
        {
            Destroy(item.gameObject);
        }
        private GameObject CreateObject(GameObject prefab, [CanBeNull] Transform container = null)
        {
            var parentContainer = container == null ? _world.Spawn.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => RemoveObject(createdGameObject)).AddTo(_disposable);
            return createdGameObject;
        }
        public void DestroyAllObjects()
        {
            _createdObjects.ForEach(Destroy);
            Dispose();
        }

        private void TryCreateDisposable()
        {
            _disposable ??= new CompositeDisposable();
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
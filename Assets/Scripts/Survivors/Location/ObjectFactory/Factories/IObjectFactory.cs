using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Location.ObjectFactory.Factories
{
    public interface IObjectFactory
    { 
        void Prepare();
        T Create<T>(string objectId, [CanBeNull] Transform container = null)
                where T : MonoBehaviour;

        T Create<T>(GameObject prefab, [CanBeNull] Transform container = null) 
                where T : MonoBehaviour;

        void Destroy<T>(GameObject instance)
                where T : MonoBehaviour;

        void DestroyAllObjects();
    }
}
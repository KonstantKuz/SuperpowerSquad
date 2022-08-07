using System;
using UnityEngine;

namespace Survivors.Location.Service
{
    public class ObjectPoolProxy: IObjectPool
    {
        private Action<object> _release;
        private Func<object> _get;
        private Action _releaseAllActive;
        

        public void Create<T>(Func<T> createFunc, 
            Action<T> onGet = null, 
            Action<T> onRelease = null, 
            Action<T> onDestroy = null,
            bool isCollectionCheck = true,
            int initialCapacity = 10,
            int maxSize = 10000,
            ObjectCreateMode objectCreateMode = ObjectCreateMode.Single,
            bool disposeActive = true) where T : MonoBehaviour
        {
            
            var pool = new ObjectPool<T>(createFunc, onGet, 
                onRelease, onDestroy, isCollectionCheck, initialCapacity, maxSize,objectCreateMode,  disposeActive); 
            
            
            
            _release = (ob) => { pool.Release((T)ob);};
            _get = () => (T) pool.Get();
            _releaseAllActive = () => pool.ReleaseAllActive();

        }

        public void ReleaseAllActive()
        {
            _releaseAllActive?.Invoke();
        }

        public T Get<T>() where T : MonoBehaviour
        {
            return (T) _get?.Invoke();
        }

        public void Release<T>(T element) where T : MonoBehaviour
        {
            _release?.Invoke(element);   
        }
    }

    public interface IObjectPool
    {

        void Create<T>(Func<T> createFunc, 
            Action<T> onGet = null, 
            Action<T> onRelease = null, 
            Action<T> onDestroy = null,
            bool isCollectionCheck = true,
            int initialCapacity = 10,
            int maxSize = 10000,
            ObjectCreateMode objectCreateMode = ObjectCreateMode.Single,
            bool disposeActive = true) where T : MonoBehaviour;

        void ReleaseAllActive();
        T Get<T>() where T : MonoBehaviour;
        
        void Release<T>(T element) where T : MonoBehaviour;
    }
}
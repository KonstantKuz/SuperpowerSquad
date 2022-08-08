using System;

namespace Survivors.ObjectPool
{
    public class ObjectPoolAdapter : IObjectPool
    {
        private Action<object> _release;
        private Func<object> _get;
        private Action _releaseAllActive;

        public void Create<T>(IObjectPool<T> objectPool)
        {
            _release = (ob) => { objectPool.Release((T) ob); };
            _get = () => (T) objectPool.Get();
            _releaseAllActive = objectPool.ReleaseAllActive;
        }

        public void ReleaseAllActive()
        {
            _releaseAllActive?.Invoke();
        }
        public T Get<T>()
        {
            return (T) _get?.Invoke();
        }
        public void Release<T>(T element)
        {
            _release?.Invoke(element);
        }
    }
}
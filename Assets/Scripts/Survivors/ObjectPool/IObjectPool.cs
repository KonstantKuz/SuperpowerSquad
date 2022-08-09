namespace Survivors.ObjectPool
{
    public interface IObjectPool<T> 
    {
        void ReleaseAllActive();
        T Get();
        void Release(T element);
    }
}
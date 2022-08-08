namespace Survivors.ObjectPool
{
    public interface IObjectPool
    {
        void ReleaseAllActive();

        T Get<T>();

        void Release<T>(T element);
    }
    
    public interface IObjectPool<T> 
    {
        void ReleaseAllActive();
        T Get();
        void Release(T element);
    }
}
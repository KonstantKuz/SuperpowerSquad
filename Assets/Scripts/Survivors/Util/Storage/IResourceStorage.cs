using UniRx;

namespace Survivors.Util.Storage
{
    public interface IResourceStorage<TK, TV>
    {
        IReactiveProperty<int> GetAsObservable(TK resource); 
        
        TV Get(TK resource);

        void Add(TK resource, TV amount);

        void Remove(TK resource, TV amount);

        bool TryRemove(TK resource, TV amount);
        
        void Set(TK resource, TV amount);

        void Reset();
        
    }
}
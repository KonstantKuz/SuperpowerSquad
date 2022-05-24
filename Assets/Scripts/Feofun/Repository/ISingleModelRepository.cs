using JetBrains.Annotations;

namespace Feofun.Repository
{
    public interface ISingleModelRepository<T>
    {
        [CanBeNull]
        T Get();
        
        T Require();
        
        bool Exists();
        
        void Set(T model);
        
        void Delete();
    }
}
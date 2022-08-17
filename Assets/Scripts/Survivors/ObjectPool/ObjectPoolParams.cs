
namespace Survivors.ObjectPool
{
    public class ObjectPoolParams
    {
        public bool IsCollectionCheck { get; set; }
        public int InitialCapacity { get; set; }
        public int MaxSize { get; set; } // todo detection Capacit
        public ObjectCreateMode ObjectCreateMode { get; set; }
        public bool DisposeActive { get; set; }

        public static ObjectPoolParams Default =>
                new ObjectPoolParams() {
                        IsCollectionCheck = true,
                        InitialCapacity = 10,
                        MaxSize = 1000,
                        ObjectCreateMode = ObjectCreateMode.Single,
                        DisposeActive = true,
                };
    }
}

namespace Survivors.ObjectPool
{
    public class ObjectPoolParams
    {
        public int InitialCapacity { get; set; }
        
        public int Capacity { get; set; }
        public int DetectionCapacity { get; set; } 
        
        public int CreationStep { get; set; }

        public static ObjectPoolParams Default =>
                new ObjectPoolParams() {
                        InitialCapacity = 10,
                        DetectionCapacity = 1000,
                };
    }
}
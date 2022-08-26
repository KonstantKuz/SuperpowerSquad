
namespace Survivors.ObjectPool
{
    public class ObjectPoolParams
    {
        public int InitialCapacity { get; set; }   
        
        public bool DetectInitialCapacityShortage { get; set; }
        
        public int MaxCapacity { get; set; } 
        
        public int SizeIncrementStep { get; set; }

        public static ObjectPoolParams Default =>
                new ObjectPoolParams() {
                        InitialCapacity = 200,
                        DetectInitialCapacityShortage = false,
                        MaxCapacity = 2000,
                        SizeIncrementStep = 1,
                };
    }
}
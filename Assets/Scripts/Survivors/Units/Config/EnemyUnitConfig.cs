using System.Runtime.Serialization;

namespace Survivors.Units.Config
{
    public class EnemyUnitConfig
    {
        [DataMember] 
        public string Id;
        [DataMember] 
        public int Health;
    }
}

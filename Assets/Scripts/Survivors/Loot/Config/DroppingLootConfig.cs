using System.Runtime.Serialization;

namespace Survivors.Loot.Config
{
    [DataContract]
    public class DroppingLootConfig
    {
        [DataMember]
        public DroppingLootType LootType;
        [DataMember]
        public int Amount;
        [DataMember]
        public float DropChance;    
        [DataMember]
        public bool AutomaticAccrual;
    }
}
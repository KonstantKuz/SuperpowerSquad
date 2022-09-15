using System.Runtime.Serialization;

namespace Survivors.Loot.Config
{
    [DataContract]
    public class DroppingLootConfig
    {
        [DataMember]
        public string LootId;
        [DataMember]
        public int Amount;
        [DataMember]
        public float DropChance;
    }
}
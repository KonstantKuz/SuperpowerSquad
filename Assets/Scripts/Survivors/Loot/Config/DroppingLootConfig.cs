using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Loot.Config
{
    [DataContract]
    public class LootEmitterConfig : ICollectionItem<string>
    {
        public string Id => EmitterId;
        
        [DataMember]
        public string EmitterId;
        [DataMember]
        public DroppingLootConfig LootConfig;
    }

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
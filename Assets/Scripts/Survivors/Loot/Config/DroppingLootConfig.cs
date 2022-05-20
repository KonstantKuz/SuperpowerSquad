using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Loot.Config
{
    [DataContract]
    public class DroppingLootConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        
        public string Id => _id;
        [DataMember]
        public DroppingLootType Type;
        [DataMember]
        public string EnemyId;
        [DataMember] 
        public int Amount;
        [DataMember] 
        public float DropChance;
    }
}
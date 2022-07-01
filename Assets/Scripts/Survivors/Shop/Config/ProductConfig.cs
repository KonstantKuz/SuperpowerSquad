using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Player.Wallet;

namespace Survivors.Shop.Config
{
    [DataContract]
    public class ProductConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private string _id;
        [DataMember(Name = "Currency")]
        public Currency Currency { get; }
        [DataMember(Name = "Cost")]
        public int Cost { get; }
        public string Id => _id;
    }
}
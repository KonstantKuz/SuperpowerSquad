using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Shop.Config
{
    [DataContract]
    public class UpgradeProductConfig : ICollectionItem<string>
    {
        [DataMember]
        public int LevelCostIncrease;
        [DataMember]
        public ProductConfig ProductConfig;
        
        public string Id => ProductConfig.Id;

        public int GetFinalCost(int level)
        {
            return ProductConfig.Cost + LevelCostIncrease * level;
        }

    }
}
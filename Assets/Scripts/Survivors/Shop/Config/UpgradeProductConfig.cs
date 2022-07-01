using System.Runtime.Serialization;

namespace Survivors.Shop.Config
{
    [DataContract]
    public class UpgradeProductConfig : ProductConfig
    {
        [DataMember]
        public int LevelCostIncrease { get; }

        public int GetFinalCost(int level)
        {
            return Cost + LevelCostIncrease * level;
        }

    }
}
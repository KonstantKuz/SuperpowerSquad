using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Squad.UpgradeSelection.Config
{
    public class UpgradeSelectionConfig : ILoadableConfig
    {
        private UpgradeSelectionConfig _config;

        [DataMember(Name = "MaxUnitUpgradesSize")]
        private int _maxUnitUpgradesSize;
        [DataMember(Name = "MaxAbilityUpgradesSize")]
        private int _maxAbilityUpgradesSize;

        public int MaxUnitUpgradesSize => _config._maxUnitUpgradesSize;

        public int MaxAbilityUpgradesSize => _config._maxAbilityUpgradesSize;

        public void Load(Stream stream)
        {
            _config = new CsvSerializer().ReadSingleObject<UpgradeSelectionConfig>(stream);
        }
    }
}
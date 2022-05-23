using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Units.Player.Upgrades
{
    public enum UpgradeType
    {
        Unit,
        Modifier
    }
    
    public class UpgradeConfig
    {
        [DataMember]
        public UpgradeType Type;
        [DataMember]
        public string UpgradeId;
    }
    
    public class UpgradesConfig: ILoadableConfig
    {
        private Dictionary<string, IReadOnlyList<UpgradeConfig>> _upgrades;

        public void Load(Stream stream)
        {
            _upgrades = new CsvSerializer().ReadNestedTable<UpgradeConfig>(stream);
        }
    }
}
using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.App.Config
{
    public class ConstantsConfig : ILoadableConfig
    {
        private ConstantsConfig _config;

        [DataMember(Name = "HealthScaleIncrementFactor")]
        private int _healthScaleIncrementFactor;

        public int HealthScaleIncrementFactor => _config._healthScaleIncrementFactor;
        public void Load(Stream stream)
        {
            _config = new CsvSerializer().ReadSingleObject<ConstantsConfig>(stream);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Units.Config
{
    public class EnemyUnitConfigs : ILoadableConfig
    {
        public IReadOnlyCollection<EnemyUnitConfig> Configs { get; private set; }
        public void Load(Stream stream)
        {
            Configs = new CsvSerializer().ReadObjectArray<EnemyUnitConfig>(stream);
        }

        public EnemyUnitConfig GetConfig(string id)
        {
            return Configs.FirstOrDefault(it => it.Id == id);
        }
    }
}
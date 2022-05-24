using System.IO;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Squad.Config
{
    public class SquadConfig : ILoadableConfig
    {
        public SquadParams Params { get; private set; }
        
        public void Load(Stream stream)
        {
            Params = new CsvSerializer().ReadSingleObject<SquadParams>(stream);
        }
    }
}
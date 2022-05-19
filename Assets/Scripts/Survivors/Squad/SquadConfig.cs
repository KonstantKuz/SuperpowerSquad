using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Squad
{
    public class SquadParams
    {
        [DataMember] 
        public float Speed;
    }

    public class SquadConfig : ILoadableConfig
    {
        public SquadParams Params { get; private set; }
        
        public void Load(Stream stream)
        {
            Params = new CsvSerializer().ReadSingleObject<SquadParams>(stream);
        }
    }
}
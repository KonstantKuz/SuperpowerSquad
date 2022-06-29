using System.IO;
using Feofun.Config.Csv;

namespace Feofun.Config
{
    public class SingleObjectConfig<TValue>: ILoadableConfig
    {
        public TValue Value { get; private set; }
        
        public void Load(Stream stream)
        {
            Value = new CsvSerializer().ReadSingleObject<TValue>(stream);
        }
    }
}
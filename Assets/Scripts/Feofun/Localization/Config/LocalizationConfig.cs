using System.Collections.Generic;
using System.IO;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Feofun.Localization.Config
{
    public class LocalizationConfig : ILoadableConfig
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Table { get; private set; }

        public void Load(Stream stream)
        {
            Table = new CsvSerializer().ReadDictionaryTable(stream);
        }
    }
}
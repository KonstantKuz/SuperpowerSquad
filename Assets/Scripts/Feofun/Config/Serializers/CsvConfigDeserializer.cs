using System;
using System.IO;
using System.Text;

namespace Feofun.Config.Serializers
{
    public class CsvConfigDeserializer: IConfigDeserializer
    {
        public T Deserialize<T>(string text) where T:ILoadableConfig
        {
            var config = Activator.CreateInstance<T>();
            var loadable = config as ILoadableConfig;
            loadable.Load(ToMemoryStream(text));
            return config;
        }
        
        public static MemoryStream ToMemoryStream(string data)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(data));
        }
    }
}
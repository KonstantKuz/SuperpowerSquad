using System;

namespace Feofun.Config.Csv
{
    public interface ICustomCsvSerializable
    {
        void Deserialize(Func<string, string> data);
    }
}
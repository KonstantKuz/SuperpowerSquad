using System.IO;

namespace Feofun.Config
{
    public interface ILoadableConfig
    {
        void Load(Stream stream);
    }
}
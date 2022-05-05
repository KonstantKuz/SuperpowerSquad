namespace Feofun.Config.Serializers
{
    public interface IConfigDeserializer
    {
        T Deserialize<T>(string text) where T:ILoadableConfig;
    }
}
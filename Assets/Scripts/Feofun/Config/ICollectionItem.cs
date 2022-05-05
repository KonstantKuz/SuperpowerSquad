namespace Feofun.Config
{
    public interface ICollectionItem<out TKey>
    {
        TKey Id { get; }
    }
}
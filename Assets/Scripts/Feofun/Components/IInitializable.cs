namespace Feofun.Components
{
    public interface IInitializable<T>
            where T : class
    {
        public void Init(T owner);
    }
}
namespace Survivors.Units
{
    public interface IInitializable<T>
            where T : class
    {
        public void Init(T owner);
    }
}

namespace Survivors.Units
{
    public interface IUnit<T>
            where T : IUnitModel
    {
        T Model { get; }
        public void Init(T model);

    }
}
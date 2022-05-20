using UniRx;

namespace Survivors.Units.Model
{
    public interface IHealthModel
    {
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
    }
}
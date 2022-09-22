using UniRx;

namespace Survivors.Units.Component
{
    public interface IHealthBarOwner
    {
        float StartingMaxValue { get; }
        IReadOnlyReactiveProperty<float> MaxValue { get; }
        IReadOnlyReactiveProperty<float> CurrentValue { get; }
    }
}
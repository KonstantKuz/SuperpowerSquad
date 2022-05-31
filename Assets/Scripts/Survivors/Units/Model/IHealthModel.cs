using UniRx;

namespace Survivors.Units.Model
{
    public interface IHealthModel
    { 
        float StartingMaxHealth { get; }
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
    }
}
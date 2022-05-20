using System;

namespace Survivors.Units.Component
{
    public interface IHealthBarOwner
    {
        float MaxValue { get; }
        IObservable<float> CurrentValue { get; }
    }
}
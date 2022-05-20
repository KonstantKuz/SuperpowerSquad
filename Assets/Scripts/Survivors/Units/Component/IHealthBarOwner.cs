using System;

namespace Survivors.Units.Component
{
    public interface IHealthBarOwner
    {
        int MaxValue { get; }
        IObservable<float> CurrentValue { get; }
    }
}
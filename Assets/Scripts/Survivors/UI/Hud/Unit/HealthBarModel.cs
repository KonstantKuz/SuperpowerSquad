using System;
using Survivors.Units.Component;
using UniRx;

namespace Survivors.UI.Hud.Unit
{
    public class HealthBarModel
    {
        public readonly IObservable<float> Percent;
        public readonly IObservable<float> MaxValue;    
        public readonly float StartingMaxValue;
        public HealthBarModel(IHealthBarOwner owner)
        {
            StartingMaxValue = owner.StartingMaxValue;
            MaxValue = owner.MaxValue;
            Percent = owner.CurrentValue.Select(it => {
                if (owner.MaxValue.Value == 0) {
                    return 0;
                }
                return 1.0f * it / owner.MaxValue.Value;
            });
        }
    }
}
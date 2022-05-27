using System;
using Survivors.Units.Component;
using Survivors.Units.Model;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Hud.Unit
{
    public class HealthBarModel
    {
        public readonly IObservable<float> Percent;    
        
        public HealthBarModel(IHealthBarOwner owner)
        {
            Percent = owner.CurrentValue.Select(it => 1.0f * it / owner.MaxValue);
        }
    }
}
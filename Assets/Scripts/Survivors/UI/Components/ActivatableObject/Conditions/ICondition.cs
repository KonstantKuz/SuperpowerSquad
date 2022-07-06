using System;

namespace Survivors.UI.Components.ActivatableObject.Conditions
{
    public interface ICondition
    {
        IObservable<bool> IsAllow();
    }
}
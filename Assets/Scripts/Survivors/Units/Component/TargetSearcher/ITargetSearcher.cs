using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.Units.Target;

namespace Survivors.Units.Component.TargetSearcher
{
    public interface ITargetSearcher
    {
        [CanBeNull]
        ITarget Find();
        // TODO: remove later. used only for multi target ranged weapon for sniper unit.
        IEnumerable<ITarget> GetAllOrderedByDistance();
    }
}
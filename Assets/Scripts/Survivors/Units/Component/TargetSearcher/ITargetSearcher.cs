using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.Units.Target;

namespace Survivors.Units.Component.TargetSearcher
{
    public interface ITargetSearcher
    {
        [CanBeNull]
        ITarget Find();
        IEnumerable<ITarget> GetAllOrderedByDistance();
    }
}
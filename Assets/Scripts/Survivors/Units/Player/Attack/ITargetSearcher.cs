using JetBrains.Annotations;
using Survivors.Units.Target;

namespace Survivors.Units.Player.Attack
{
    public interface ITargetSearcher
    {
        [CanBeNull]
        ITarget Find();
    }
}
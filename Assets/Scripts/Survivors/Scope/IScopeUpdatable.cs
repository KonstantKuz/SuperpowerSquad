using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public interface IUpdatableScope
    {
        ITimer Timer { get; }
        ICoroutineRunner CoroutineRunner { get; }
    }
}
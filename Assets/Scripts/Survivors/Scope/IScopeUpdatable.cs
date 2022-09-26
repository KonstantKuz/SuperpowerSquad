using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public interface IUpdatableScope
    {
        IScopeTime ScopeTime { get; }
        ICoroutineRunner CoroutineRunner { get; }
    }
}
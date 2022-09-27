using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public interface IUpdatableScope
    {
        bool IsPaused { get; set; }
        IScopeTime ScopeTime { get; }
        ICoroutineRunner CoroutineRunner { get; }

    }
}
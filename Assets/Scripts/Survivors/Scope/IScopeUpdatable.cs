using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public interface IScopeUpdatable
    {
        ITimer Timer { get; }
        ICoroutineRunner CoroutineRunner { get; }
    }
}
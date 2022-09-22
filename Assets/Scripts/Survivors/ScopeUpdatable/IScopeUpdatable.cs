using Survivors.ScopeUpdatable.Coroutine;
using Survivors.ScopeUpdatable.Timer;

namespace Survivors.ScopeUpdatable
{
    public interface IScopeUpdatable
    {
        ITimer Timer { get; }
        ICoroutineRunner CoroutineRunner { get; }
    }
}
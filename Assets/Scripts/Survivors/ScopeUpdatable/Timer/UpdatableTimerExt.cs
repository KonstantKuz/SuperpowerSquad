
using Survivors.ScopeUpdatable.Coroutine;

namespace Survivors.ScopeUpdatable.Timer
{
    public static class UpdatableTimerExt
    {
        public static ICoroutineRunner CreateCoroutineRunner(this ITimer timer) => new CoroutineRunner(timer);
    }
}
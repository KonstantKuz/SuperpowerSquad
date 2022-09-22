
using Survivors.Scope.Coroutine;

namespace Survivors.Scope.Timer
{
    public static class UpdatableTimerExt
    {
        public static ICoroutineRunner CreateCoroutineRunner(this ITimer timer) => new CoroutineRunner(timer);
    }
}
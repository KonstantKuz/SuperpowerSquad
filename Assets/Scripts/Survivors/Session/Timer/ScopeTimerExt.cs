
namespace Survivors.Session.Timer
{
    public static class ScopeTimerExt
    {
        public static ICoroutineRunner CreateCoroutineRunner(this ITimer timer) => new CoroutineRunner(timer);
    }
}
namespace Survivors.Session.Timer
{
    public interface IScopeUpdatable
    {
        ITimer Timer { get; }
        ICoroutineRunner CoroutineRunner { get; }
    }
}
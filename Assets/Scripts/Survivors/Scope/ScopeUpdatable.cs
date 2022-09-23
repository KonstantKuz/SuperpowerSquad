using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public class ScopeUpdatable : IScopeUpdatable
    {
        private readonly UpdatableTimer _timer;
        private bool _isPaused;
        public ITimer Timer => _timer;
        public ICoroutineRunner CoroutineRunner { get; }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                _timer.SetPause(_isPaused);
            }
        }

        public ScopeUpdatable()
        {
            _timer = new UpdatableTimer();
            CoroutineRunner = Timer.CreateCoroutineRunner();
        }

        public void Reset() => _timer.Reset();

        public void Update(float deltaTime)
        {
            if (IsPaused) return;
            _timer.Update(deltaTime);
        }
    }
}
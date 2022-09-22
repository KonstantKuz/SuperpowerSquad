
namespace Survivors.Session.Timer
{
    public class ScopeUpdatable : IScopeUpdatable
    {
        private readonly UpdatableTimer _timer;
        private bool _pause;
        public ITimer Timer => _timer;
        public ICoroutineRunner CoroutineRunner { get; }

        public bool Pause
        {
            get => _pause;
            set
            {
                _pause = value;
                _timer.SetPause(_pause);
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
            if (Pause) return;
            _timer.Update(deltaTime);
        }
    }
}
using System;

namespace Feofun.App.Init
{
    public abstract class AppInitStep
    {
        private Action _onNext;
        
        public void Run(Action onNext)
        {
            _onNext = onNext;
            Run();
        }
        protected abstract void Run();
        protected void Next()
        {
            _onNext?.Invoke();
            _onNext = null;
        }
    }
}
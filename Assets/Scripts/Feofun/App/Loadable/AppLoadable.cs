using System;

namespace Feofun.App.Loadable
{
    public abstract class AppLoadable
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
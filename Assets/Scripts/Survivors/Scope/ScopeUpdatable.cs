using System;
using System.Collections;
using Survivors.Scope.Coroutine;
using Survivors.Scope.Timer;

namespace Survivors.Scope
{
    public class UpdatableScope : IUpdatableScope, ICoroutineRunner, IScopeTime
    {
        public bool IsPaused { get; set; }
        public IScopeTime ScopeTime => this;
        public ICoroutineRunner CoroutineRunner { get; }
        public float Time { get; private set; }
        public event Action OnTick;
        public UpdatableScope()
        {
            CoroutineRunner = new CoroutineRunner(this);
        }
        public ICoroutine StartCoroutine(IEnumerator coroutine) => CoroutineRunner.StartCoroutine(coroutine);

        public void StopCoroutine(ICoroutine coroutine) => CoroutineRunner.StopCoroutine(coroutine);
        
        public void Reset() => Time = 0;
        
        public void Update()
        {
            if (IsPaused) return;
            Time += UnityEngine.Time.deltaTime;
            OnTick?.Invoke();
        }

      
    }
}
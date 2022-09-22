using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using Survivors.ScopeUpdatable.Timer;

namespace Survivors.ScopeUpdatable.Coroutine
{
    public class CoroutineRunner : IDisposable, ICoroutineRunner
    {
        private readonly ISet<CoroutineEntity> _coroutines = new HashSet<CoroutineEntity>();
        private readonly ITimer _timer;
        public CoroutineRunner(ITimer timer)
        {
            _timer = timer;
            _timer.OnUpdate += OnUpdate;
        }
        public ICoroutine StartCoroutine(IEnumerator coroutine)
        {
            var coroutineEntity = new CoroutineEntity(coroutine);
            _coroutines.Add(coroutineEntity);
            return coroutineEntity;
        }

        public void StopCoroutine(ICoroutine coroutine)
        {
            var coroutineEntity = (CoroutineEntity) coroutine;
            coroutineEntity.Stop();
            _coroutines.Remove(coroutineEntity);
        }

        public void Dispose() => _timer.OnUpdate -= OnUpdate;
        private void OnUpdate()
        {
            if (_coroutines.IsEmpty()) { 
                return;
            }
            var removeCoroutine = new List<CoroutineEntity>();
            foreach (var coroutineEntity in _coroutines) {
                if (!coroutineEntity.MoveNext()) {
                    removeCoroutine.Add(coroutineEntity);
                }
            }
            removeCoroutine.ForEach(it => {
                _coroutines.Remove(it);
            });
        }
    }
}
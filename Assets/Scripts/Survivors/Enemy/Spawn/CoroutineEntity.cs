using System.Collections;
using System.Collections.Generic;

namespace Survivors.Enemy.Spawn
{
    public class WaitForSeconds : IEnumerator
    {
        private ITimer _timer;
        private float _timeout;

        private float _leftTime;
        public WaitForSeconds(ITimer timer, float timeout)
        {
            _timer = timer;
            _timeout = timeout;
        }

        public bool MoveNext()
        {
            _leftTime += _timer.DeltaTime;
            return _leftTime < _timeout;
        }

        public void Reset()
        {
            _leftTime = 0;
        }

        public object Current => (object) null;
    }

    public class CoroutineEntity
    {
        private readonly Stack<IEnumerator> _enumerators = new Stack<IEnumerator>();
        
        private IEnumerator _currentCoroutine;

        public bool IsComplete { get; private set; }

        public CoroutineEntity(IEnumerator coroutine)
        {
            _currentCoroutine = coroutine;
        }
        public void Update()
        {
            if (_currentCoroutine == null) {
                IsComplete = true;
                return;
            }
            if (_currentCoroutine.MoveNext())
            {
                if (!(_currentCoroutine.Current is IEnumerator nestedCoroutine)) return;
                _enumerators.Push(_currentCoroutine);
                _currentCoroutine = nestedCoroutine;
            } else {
                _currentCoroutine = _enumerators.Count > 0 ? _enumerators.Pop() : null;
            }
        }
    }
}
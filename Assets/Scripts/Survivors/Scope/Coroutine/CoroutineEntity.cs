using System.Collections;
using System.Collections.Generic;

namespace Survivors.Scope.Coroutine
{
    public class CoroutineEntity : IEnumerator, ICoroutine
    {
        private readonly Stack<IEnumerator> _enumerators = new Stack<IEnumerator>();
        
        private IEnumerator _currentCoroutine;

        public bool IsComplete { get; private set; }

        public CoroutineEntity(IEnumerator coroutine)
        {
            _currentCoroutine = coroutine;
        }
        public void Stop() => IsComplete = true;

        public bool MoveNext()
        {
            if (IsComplete) {
                return false;
            }
            if (_currentCoroutine == null) {
                IsComplete = true;
                return false;
            }
            if (_currentCoroutine.MoveNext())
            {
                if (!(_currentCoroutine.Current is IEnumerator nestedCoroutine)) return true;
                _enumerators.Push(_currentCoroutine);
                _currentCoroutine = nestedCoroutine;
       
            } else {
                _currentCoroutine = _enumerators.Count > 0 ? _enumerators.Pop() : null;
            }
            return true;
        }

        public void Reset() { }

        public object Current => _currentCoroutine;
    }
}
using System.Collections;
using System.Collections.Generic;

namespace Survivors.Scope.Coroutine
{
    public class CoroutineEntity : IEnumerator, ICoroutine
    {
        private readonly Stack<IEnumerator> _enumerators = new Stack<IEnumerator>();
        
        private IEnumerator _currentCoroutine;

        private bool _isComplete;

        public CoroutineEntity(IEnumerator coroutine)
        {
            _currentCoroutine = coroutine;
        }
        public void Stop() => _isComplete = true;

        public bool MoveNext()
        {
            if (_isComplete) {
                return false;
            }
            if (_currentCoroutine == null) {
                _isComplete = true;
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
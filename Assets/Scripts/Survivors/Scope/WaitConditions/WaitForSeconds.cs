using System.Collections;
using Survivors.Scope.Timer;

namespace Survivors.Scope.WaitConditions
{
    public class WaitForSeconds : IEnumerator
    {
        private readonly float _timeout;      
        private readonly IScopeTime _scopeTime;

        private float _startTime;
        public WaitForSeconds(IScopeTime scopeTime, float timeout)
        {
            _scopeTime = scopeTime;
            _timeout = timeout;
            _startTime = _scopeTime.Time;
        }

        public bool MoveNext() => _scopeTime.Time - _startTime < _timeout;

        public void Reset()
        {
            _startTime = 0;
        }
        public object Current => null;
    }
}
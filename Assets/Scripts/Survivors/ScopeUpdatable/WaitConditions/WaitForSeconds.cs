using System.Collections;
using Survivors.ScopeUpdatable.Timer;

namespace Survivors.ScopeUpdatable.WaitConditions
{
    public class WaitForSeconds : IEnumerator
    {
        private readonly ITimer _timer;
        private readonly float _timeout;

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
}
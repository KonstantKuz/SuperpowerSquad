using System.Collections;
using UnityEngine;


namespace Survivors.Scope.WaitConditions
{
    public class WaitForSeconds : IEnumerator
    {
        private readonly float _timeout;

        private float _leftTime;
        public WaitForSeconds(float timeout)
        {
            _timeout = timeout;
        }

        public bool MoveNext()
        {
            _leftTime += Time.deltaTime;
            return _leftTime < _timeout;
        }
        public void Reset()
        {
            _leftTime = 0;
        }
        public object Current => (object) null;
    }
}
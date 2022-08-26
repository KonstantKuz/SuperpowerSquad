using System.Collections;
using Feofun.Components;
using UniRx;
using UnityEngine;

namespace Survivors.Session.Model
{
    public class PlayTimer
    {
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _counter;
        public FloatReactiveProperty PlayTime { get; }

        public PlayTimer(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _counter = coroutineRunner.StartCoroutine(Timer());
            PlayTime = new FloatReactiveProperty(0);
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                PlayTime.Value++;
            }
        }

        public void Stop()
        {
            _coroutineRunner.StopCoroutine(_counter);
        }
    }
}
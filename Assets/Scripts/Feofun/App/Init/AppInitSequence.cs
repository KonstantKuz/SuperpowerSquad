using System.Collections.Generic;
using UnityEngine;

namespace Feofun.App.Init
{
    public class AppInitSequence : MonoBehaviour
    {
        private readonly Queue<AppInitStep> _steps = new Queue<AppInitStep>();

        public void AddStep<T>() where T : AppInitStep
        {
            var loadable = AppContext.Container.Instantiate<T>();
            _steps.Enqueue(loadable);
        }
        public void AddStep<T>(IEnumerable<object> extraArgs) where T : AppInitStep
        {
            var step = AppContext.Container.Instantiate<T>(extraArgs);
            _steps.Enqueue(step);
        }

        public void Next()
        {
            if (_steps.Count == 0) {
                Destroy(this);
                return;
            }
            var step = _steps.Dequeue();
            Debug.Log($"AppInitSequence run step= {step.GetType().Name}");
            step.Run(Next);
        }
    }
}
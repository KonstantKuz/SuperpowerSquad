using System;

namespace Survivors.Scope.Timer
{
    public class UpdatableTimer : ITimer
    {
        public bool IsPaused { get; private set; }    
        public float Time { get; private set; }

        public event Action<float> OnUpdate;
        
        public void Reset()
        {
            Time = 0;
        }

        public void SetPause(bool isPaused)
        {
            IsPaused = isPaused;
        }
        public void Update(float deltaTime)
        {
            if (IsPaused) {
                return;
            }
            Time += deltaTime;
            OnUpdate?.Invoke(deltaTime);
        }
    }
}
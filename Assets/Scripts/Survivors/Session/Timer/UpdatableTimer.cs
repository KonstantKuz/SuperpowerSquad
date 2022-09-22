using System;

namespace Survivors.Session.Timer
{
    public class UpdatableTimer : ITimer
    {
        public bool Pause { get; private set; }    
        public float Time { get; private set; }      
        public float DeltaTime { get; private set; }
        
        public event Action OnUpdate;
        
        public void Reset()
        {
            Time = 0;
            DeltaTime = 0;
        }

        public void SetPause(bool pause)
        {
            Pause = pause;
        }

        public void Update(float deltaTime)
        {
            DeltaTime = 0;
            if (Pause) {
                return;
            }
            DeltaTime = deltaTime;
            Time += deltaTime;
            OnUpdate?.Invoke();
        }
    }
}
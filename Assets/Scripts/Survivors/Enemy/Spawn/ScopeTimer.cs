using System.Collections;

namespace Survivors.Enemy.Spawn
{
    public class ScopeTimer : ITimer
    {
        public bool Pause { get; set; }
        public float Time { get; private set; }      
        public float DeltaTime { get; private set; }
        
        private CoroutineEntity _coroutineEntity;

        public ScopeTimer()
        {
            Time = 0;
        }
        public IEnumerator StartCoroutine(IEnumerator coroutine)
        {
            _coroutineEntity = new CoroutineEntity(coroutine);
            return coroutine;
        }    
        public void StopCoroutine(IEnumerator coroutine)
        {
            _coroutineEntity = null;
        }

        public void Update(float deltaTime)
        {
            DeltaTime = 0;
            if (Pause) {
                return;
            }
            Time += deltaTime;
            DeltaTime = deltaTime;

            if (_coroutineEntity != null) {
                _coroutineEntity.Update();
            }
            if (_coroutineEntity != null && _coroutineEntity.IsComplete) {
                _coroutineEntity = null;
            }
        }
    }
    
    public interface ITimer
    { 
        bool Pause { get; set; } 
        float Time { get; } 
        float DeltaTime { get; }
    }
}
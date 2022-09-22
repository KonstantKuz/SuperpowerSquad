using System.Collections;

namespace Survivors.ScopeUpdatable.Coroutine
{
    public interface ICoroutineRunner
    { 
        ICoroutine StartCoroutine(IEnumerator coroutine); 
        
        void StopCoroutine(ICoroutine coroutine);
    }
}
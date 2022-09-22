using System.Collections;

namespace Survivors.Scope.Coroutine
{
    public interface ICoroutineRunner
    { 
        ICoroutine StartCoroutine(IEnumerator coroutine); 
        
        void StopCoroutine(ICoroutine coroutine);
    }
}
using System.Collections;

namespace Survivors.Session.Timer
{
    public interface ICoroutineRunner
    { 
        CoroutineEntity StartCoroutine(IEnumerator coroutine); 
        
        void StopCoroutine(CoroutineEntity coroutine);
    }
}
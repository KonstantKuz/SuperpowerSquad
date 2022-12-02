using System.Collections;
using UnityEngine;

namespace Survivors.Util
{
    public class CoroutineUtil
    {
        public static IEnumerator WaitForSecondsFixedTime(float time, bool considerTimeScale = false)
        {
            var fixedDeltaTime = considerTimeScale ? Time.fixedDeltaTime * Time.timeScale : Time.fixedDeltaTime;
            var framesCount = time / fixedDeltaTime;
            for (int i = 0; i < framesCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
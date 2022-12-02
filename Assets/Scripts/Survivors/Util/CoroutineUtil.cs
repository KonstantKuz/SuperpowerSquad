using System.Collections;
using UnityEngine;

namespace Survivors.Util
{
    public class CoroutineUtil
    {
        public static IEnumerator WaitForSecondsFixedTime(float time)
        {
            var framesCount = time / (Time.timeScale * Time.fixedDeltaTime);
            for (int i = 0; i < framesCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
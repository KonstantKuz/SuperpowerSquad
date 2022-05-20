using System.Collections;
using System.Collections.Generic;

namespace Feofun.Extension
{
    public static class CoroutineExt
    {
        public static void RunBlocking(this IEnumerator coroutine)
        {
            var enumerators = new Stack<IEnumerator>();
            var currentCoroutine = coroutine;
            while (currentCoroutine != null)
            {
                while (currentCoroutine.MoveNext())
                {
                    if (!(currentCoroutine.Current is IEnumerator nestedCoroutine)) continue;
                    enumerators.Push(currentCoroutine);
                    currentCoroutine = nestedCoroutine;
                }

                currentCoroutine = enumerators.Count > 0 ? enumerators.Pop() : null;
            }
        }
    }
}
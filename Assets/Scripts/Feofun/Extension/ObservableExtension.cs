using System;
using UniRx;

namespace Feofun.Extension
{
    public static class ObservableExtension
    {
        public static IObservable<float> Diff(this IObservable<float> source) =>
            source.Buffer(2, 1).Select(it => it[1] - it[0]);
    }
}
using System;
using DG.Tweening;

namespace Feofun.Extension
{
    public static class TweenExt
    {
        public static IDisposable ToDisposable(this Tween tween, bool completeOnKill = false)
        {
            return new DisposableTween(tween, completeOnKill);
        }
    }

    public class DisposableTween : IDisposable
    {
        private readonly Tween _tween;
        private readonly bool _completeOnKill;
        
        public DisposableTween(Tween tween, bool completeOnKill = false)
        {
            _tween = tween;
            _completeOnKill = completeOnKill;
        }

        public void Dispose()
        {
            _tween.Kill(_completeOnKill);
        }
    }
}
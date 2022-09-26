using System;

namespace Feofun.Extension
{
    public static class ActionExtension
    {
        public static IDisposable SubscribeWithDisposable<T>(this Action<T> action, Action<T> subscriber)
        {
            return new ActionSubscription<T>(action, subscriber);
        }
        
        private class ActionSubscription<T> : IDisposable
        {
            private Action<T> _action;
            private readonly Action<T> _subscriber;
            
            public ActionSubscription(Action<T> action, Action<T> subscriber)
            {
                _action = action;
                _subscriber = subscriber;

                _action += _subscriber;
            }
            
            public void Dispose()
            {
                _action -= _subscriber;
            }
        }
    }
}
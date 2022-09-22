using System;
using SuperMaxim.Messaging;

namespace Feofun.Extension
{
    public static class DisposableExtension
    {
        public static IDisposable SubscribeWithDisposable<T>(this IMessenger messenger, Action<T> func)
        {
            return new MessageSubscription<T>(messenger, func);
        }

        public static IDisposable SubscribeWithDisposable<T>(this Action<T> action, Action<T> subscriber)
        {
            return new ActionSubscription<T>(action, subscriber);
        }
        
        private class MessageSubscription<T> : IDisposable
        {
            private readonly IMessenger _messenger;
            private readonly Action<T> _func;
            public MessageSubscription(IMessenger messenger, Action<T> func)
            {
                _messenger = messenger;
                _func = func;
                _messenger.Subscribe(_func);
            }

            public void Dispose()
            {
                _messenger.Unsubscribe(_func);
            }
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
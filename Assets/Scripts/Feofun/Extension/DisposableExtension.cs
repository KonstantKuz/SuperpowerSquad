using System;
using SuperMaxim.Messaging;

namespace Feofun.Extension
{
    public static class DisposableExt
    {
        public static IDisposable SubscribeWithDisposable<T>(this IMessenger messenger, Action<T> func)
        {
            return new MessageSubscription<T>(messenger, func);
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
    }
}
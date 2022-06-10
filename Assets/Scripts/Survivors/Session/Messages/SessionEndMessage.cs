using Survivors.Session.Model;

namespace Survivors.Session.Messages
{
    public readonly struct SessionEndMessage
    {
        public readonly SessionResult Result;

        public SessionEndMessage(SessionResult result)
        {
            Result = result;
        }
    }
}
namespace Survivors.Session.Messages
{
    public readonly struct SessionStartMessage
    {
        public readonly int Level;

        public SessionStartMessage(int level)
        {
            Level = level;
        }
    }
}
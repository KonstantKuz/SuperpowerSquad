using Survivors.WorldEvents.Config;

namespace Survivors.WorldEvents.Messages
{
    public readonly struct WorldEventStartMessage
    {
        public readonly WorldEventType EventType;

        public WorldEventStartMessage(WorldEventType eventType)
        {
            EventType = eventType;
        }
    }
}
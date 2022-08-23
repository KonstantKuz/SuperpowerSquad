using Survivors.WorldEvents.Config;

namespace Survivors.WorldEvents.Messages
{
    public readonly struct WorldEventStartTimerMessage
    {
        public readonly WorldEventType EventType;  
        public readonly float TimeBeforeShowEvent;

        public WorldEventStartTimerMessage(WorldEventType eventType, float timeBeforeShowEvent)
        {
            EventType = eventType;
            TimeBeforeShowEvent = timeBeforeShowEvent;
        }
    }
}
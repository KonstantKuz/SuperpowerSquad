using Survivors.WorldEvents.Config;

namespace Survivors.WorldEvents.Messages
{
    public readonly struct WorldEventTimerStartMessage
    {
        public readonly WorldEventType EventType;  
        public readonly float TimeBeforeShowEvent;

        public WorldEventTimerStartMessage(WorldEventType eventType, float timeBeforeShowEvent)
        {
            EventType = eventType;
            TimeBeforeShowEvent = timeBeforeShowEvent;
        }
    }
}
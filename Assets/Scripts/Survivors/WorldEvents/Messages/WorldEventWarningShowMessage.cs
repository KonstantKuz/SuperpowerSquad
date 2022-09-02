using Survivors.WorldEvents.Config;

namespace Survivors.WorldEvents.Messages
{
    public readonly struct WorldEventWarningShowMessage
    {
        public readonly WorldEventType EventType;
        public readonly float EventWarningShowDuration;

        public WorldEventWarningShowMessage(WorldEventType eventType, float eventWarningShowDuration)
        {
            EventType = eventType;
            EventWarningShowDuration = eventWarningShowDuration;
        }
    }
}
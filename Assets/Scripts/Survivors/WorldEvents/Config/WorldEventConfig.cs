using System.Runtime.Serialization;

namespace Survivors.WorldEvents.Config
{
    [DataContract]
    public class WorldEventConfig
    {
        [DataMember(Name = "EventType")]
        public WorldEventType EventType;

        [DataMember(Name = "TimeoutBeforeEvent")]
        public float TimeoutBeforeEvent;
    }
}
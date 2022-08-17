using System.Runtime.Serialization;

namespace Survivors.WorldEvents.Config
{
    [DataContract]
    public class WorldEventConfig
    {
        [DataMember(Name = "WorldEventType")]
        public WorldEventType EventType;

        [DataMember(Name = "TimeSincePreviousEvent")]
        public float TimeSincePreviousEvent;
    }
}
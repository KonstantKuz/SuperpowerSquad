using System.Runtime.Serialization;

namespace Survivors.Squad.Config
{
    public class SquadParams
    {
        [DataMember] 
        public float Speed;
        [DataMember]
        public float CollectRadius;
    }
}
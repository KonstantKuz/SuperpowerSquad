using System.Runtime.Serialization;

namespace Survivors.Squad.Config
{
    public class SquadConfig
    {
        [DataMember] 
        public float Speed;
        [DataMember]
        public float InitialRadius;
        [DataMember] 
        public float RadiusIncreaseStep;
        [DataMember]
        public float AttackDistance;   
        [DataMember] 
        public float TokenRegeneration; 
        [DataMember] 
        public float ExpRegeneration;
        [DataMember] 
        public float HealthRegeneration;
    }
}
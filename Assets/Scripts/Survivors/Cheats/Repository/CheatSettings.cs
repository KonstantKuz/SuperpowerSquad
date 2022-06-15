using System.Runtime.Serialization;

namespace Survivors.Cheats.Repository
{
    [DataContract]
    public class CheatSettings
    {
        [DataMember]
        public bool ConsoleEnabled; 
        [DataMember]
        public bool FPSMonitorEnabled;
        
    }
}
using System.Runtime.Serialization;

namespace Survivors.Cheats.Data
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
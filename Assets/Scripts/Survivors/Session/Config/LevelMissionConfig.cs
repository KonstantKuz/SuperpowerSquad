using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Session.Config
{
    [DataContract]
    public class LevelMissionConfig : ICollectionItem<string>
    {
        [DataMember]
        public int Level;
        [DataMember] 
        public LevelMissionType MissionType;
        [DataMember]
        public int KillCount;
        [DataMember] 
        public int Time;
        
        public string Id => Level.ToString();
    }
}
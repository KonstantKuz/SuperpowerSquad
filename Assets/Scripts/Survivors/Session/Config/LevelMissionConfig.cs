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
        public int KillCount;
        public string Id => Level.ToString();
    }
}
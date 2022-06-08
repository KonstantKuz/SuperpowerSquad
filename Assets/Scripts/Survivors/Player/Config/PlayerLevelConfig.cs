using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Player.Config
{
    [DataContract]
    public class PlayerLevelConfig : ICollectionItem<string>
    {
        [DataMember]
        public int Level;
        [DataMember]
        public int KillCountToNextLevel;
        public string Id => Level.ToString();
    }
}
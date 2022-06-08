using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Enemy.Config
{
    [DataContract]
    public class EnemyLevelConfig : ICollectionItem<string>
    {
        [DataMember]
        public int Level;
        [DataMember]
        public int KillCount;
        public string Id => Level.ToString();
    }
}
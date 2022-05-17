using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        public string _id;
        [DataMember(Name = "Health")] 
        public int _health;

        public string Id => _id;
        public int Health => _health;
    }
}

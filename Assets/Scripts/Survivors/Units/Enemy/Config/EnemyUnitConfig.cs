using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember(Name = "Health")] 
        private int _health;

        public string Id => _id;
        public int Health => _health;
    }
}

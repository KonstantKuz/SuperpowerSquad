using System.Runtime.Serialization;

namespace Survivors.Enemy.Spawn.Config
{
    public class GroupsSpawnerConfig
    {
        [DataMember] 
        public int MinGroupsCount;
        [DataMember] 
        public int MaxGroupsCount;
    }
}
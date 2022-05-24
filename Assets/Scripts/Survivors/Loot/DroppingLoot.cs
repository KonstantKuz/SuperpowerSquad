using Survivors.Location.Model;
using Survivors.Loot.Config;

namespace Survivors.Loot
{
    public class DroppingLoot : WorldObject
    {
        public DroppingLootConfig Config { get; private set; }
        public void Init(DroppingLootConfig config)
        {
            Config = config;
        }
    }
}
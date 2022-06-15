using Feofun.Repository;
using Survivors.Cheats.Repository;

namespace Survivors.Cheats
{
    public class CheatRepository : LocalPrefsSingleRepository<CheatSettings>
    {
        public CheatRepository() : base("CheatSettings")
        {
        }
    }
}
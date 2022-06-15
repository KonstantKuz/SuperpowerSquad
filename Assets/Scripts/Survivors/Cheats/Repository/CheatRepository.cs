using Feofun.Repository;
using Survivors.Cheats.Data;

namespace Survivors.Cheats.Repository
{
    public class CheatRepository : LocalPrefsSingleRepository<CheatSettings>
    {
        public CheatRepository() : base("CheatSettings")
        {
        }
    }
}
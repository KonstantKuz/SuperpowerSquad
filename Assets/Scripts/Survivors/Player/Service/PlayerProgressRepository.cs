using Feofun.Repository;
using Survivors.Player.Model;

namespace Survivors.Player.Service
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}
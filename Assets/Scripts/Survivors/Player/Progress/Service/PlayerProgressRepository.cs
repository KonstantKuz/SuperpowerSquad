using Feofun.Repository;
using Survivors.Player.Progress.Model;

namespace Survivors.Player.Progress.Service
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}
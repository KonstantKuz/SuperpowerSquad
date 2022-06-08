using Feofun.Repository;

namespace Survivors.Player.Progress
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}
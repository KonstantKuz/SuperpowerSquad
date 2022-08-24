using Feofun.Repository;
using Survivors.Upgrade.MetaUpgrade.Data;

namespace Survivors.Upgrade.MetaUpgrade
{
    public class MetaUpgradeRepository : LocalPrefsSingleRepository<UnitsMetaUpgrades>
    {
        protected MetaUpgradeRepository() : base("UnitsMetaUpgrades_v1")
        {
        }
    }
}
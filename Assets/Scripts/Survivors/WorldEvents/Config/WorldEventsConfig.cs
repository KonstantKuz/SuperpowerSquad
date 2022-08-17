using System.Collections.Generic;
using Survivors.Squad.Upgrade.Config;

namespace Survivors.WorldEvents.Config
{
    public class WorldEventsConfig
    {
        private IReadOnlyDictionary<int, UpgradeBranchConfig> _levelEvents;
    }
}
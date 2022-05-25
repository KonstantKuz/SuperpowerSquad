using System.Collections.Generic;
using System.Linq;

namespace Survivors.UI.Dialog
{
    public class UpgradeDialogInitModel
    {
        private List<string> _upgradeBranchIds;

        public UpgradeDialogInitModel(List<string> upgradeBranchIds)
        {
            _upgradeBranchIds = upgradeBranchIds.ToList();
        }

        public List<string> UpgradeBranchIds => _upgradeBranchIds;
    }
}
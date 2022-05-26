using System;
using System.Collections.Generic;
using System.Linq;

namespace Survivors.UI.Dialog.Model
{
    public class UpgradeDialogInitModel
    {
        private List<string> _upgradeBranchIds;      
        private Action<string> _onUpgrade;

        public UpgradeDialogInitModel(List<string> upgradeBranchIds, Action<string> onUpgrade)
        {
            _upgradeBranchIds = upgradeBranchIds.ToList();
            _onUpgrade = onUpgrade;
        }

        public Action<string> OnUpgrade => _onUpgrade;
        public List<string> UpgradeBranchIds => _upgradeBranchIds;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeDialogInitModel
    {
        private int _level;
        private List<string> _upgradeBranchIds;
        private Action<string> _onUpgrade;

        public UpgradeDialogInitModel(int level, List<string> upgradeBranchIds, Action<string> onUpgrade)
        {
            _level = level;
            _upgradeBranchIds = upgradeBranchIds.ToList();
            _onUpgrade = onUpgrade;
        }

        public int Level => _level;
        public List<string> UpgradeBranchIds => _upgradeBranchIds;
        public Action<string> OnUpgrade => _onUpgrade;
    }
}
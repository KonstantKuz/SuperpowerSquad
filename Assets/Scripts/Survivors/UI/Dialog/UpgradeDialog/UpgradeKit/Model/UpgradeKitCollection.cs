using System.Collections.Generic;
using System.Linq;
using Survivors.Upgrade.UpgradeSelection;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model
{
    public class UpgradeKitCollection
    {
        public UpgradeBranchType BranchType { get; }

        private IEnumerable<UpgradeKitItemModel> _items;

        public IEnumerable<UpgradeKitItemModel> Items
        {
            get => BranchType == UpgradeBranchType.Ability ? _items : _items.Reverse();
            private set => _items = value;
        }

        public UpgradeKitCollection(UpgradeBranchType branchType, IEnumerable<UpgradeKitItemModel> items)
        {
            BranchType = branchType;
            Items = items;
        }
    }
}
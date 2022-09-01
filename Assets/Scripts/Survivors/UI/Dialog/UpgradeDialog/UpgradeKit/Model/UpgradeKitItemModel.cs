using JetBrains.Annotations;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model
{
    public class UpgradeKitItemModel
    {
        [CanBeNull]
        public string Id { get; set; }
        public UpgradeKitViewState State { get; set; }
        
        public UpgradeKitItemModel(UpgradeKitViewState state, [CanBeNull] string id = null)
        {
            Id = id;
            State = state;
        }

    }
}
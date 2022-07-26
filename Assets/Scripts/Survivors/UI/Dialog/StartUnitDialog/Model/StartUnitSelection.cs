namespace Survivors.UI.Dialog.StartUnitDialog.Model
{
    public readonly struct StartUnitSelection
    {
        public readonly string UnitId;
        public readonly string UpgradeId;

        public StartUnitSelection(string unitId, string upgradeId)
        {
            UnitId = unitId;
            UpgradeId = upgradeId;
        }
    }
}
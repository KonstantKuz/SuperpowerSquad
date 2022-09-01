namespace Survivors.Upgrade.Messages
{
    public readonly struct UpgradeAppliedMessage
    {
        public readonly string UpgradeBranchId;

        public UpgradeAppliedMessage(string upgradeBranchId)
        {
            UpgradeBranchId = upgradeBranchId;
        }
    }
}
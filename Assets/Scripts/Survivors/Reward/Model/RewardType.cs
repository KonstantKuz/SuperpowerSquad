namespace Survivors.Reward.Model
{
    public enum RewardType
    {
        None,
        Currency,

    }
    public static class RewardTypeExtension
    {
        /*public static string GetIconPath(this RewardItem rewardItem)
        {
            return GetIconPath(rewardItem.RewardType, rewardItem.RewardId);
        }*/
        /*public static string GetIconPath(RewardType rewardType, string rewardId)
        {
            return rewardType switch {
                    RewardType.Shards => IconPath.GetUnitVertical(rewardId),
                    RewardType.LootBox => IconPath.GetReward(RewardType.LootBox.ToString()),
                    _ => IconPath.GetReward(rewardId)
            };
        }*/
    }
}
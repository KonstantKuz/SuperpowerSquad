using System.Collections.Generic;
using Survivors.Reward.Model;

namespace Survivors.Reward.Service
{
    public interface IRewardApplyService
    {
        void ApplyReward(RewardItem rewardItem);
        public void ApplyRewards(IEnumerable<RewardItem> items);
    }
}
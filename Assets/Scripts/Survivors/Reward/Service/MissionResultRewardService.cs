using System.Collections.Generic;
using Feofun.Config;
using JetBrains.Annotations;
using Survivors.Player.Wallet;
using Survivors.Reward.Config;
using Survivors.Reward.Model;
using Survivors.Session.Model;

namespace Survivors.Reward.Service
{
    [PublicAPI]
    public class MissionResultRewardService
    {
        private readonly ConfigCollection<SessionResult, MissionRewardsConfig> _missionRewards;

        public List<RewardItem> CalculateRewards(SessionResult result, Session.Model.Session session)
        {
            var missionConfig = _missionRewards.Get(result);
            var rewardCount = (int) (session.Kills * missionConfig.KilledFactor);
            var reward = new RewardItem(Currency.Soft.ToString(), RewardType.Currency, rewardCount);
            return new List<RewardItem>() {
                    reward
            };
        }
    }
}
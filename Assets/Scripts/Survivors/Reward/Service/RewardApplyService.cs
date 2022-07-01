using System;
using System.Collections.Generic;
using Feofun.Extension;
using JetBrains.Annotations;
using Survivors.Player.Wallet;
using Survivors.Reward.Model;
using Zenject;

namespace Survivors.Reward.Service
{
    [PublicAPI]
    public class RewardApplyService : IRewardApplyService
    {
        [Inject]
        private WalletService _walletService;
        public void ApplyReward(RewardItem rewardItem)
        {
            switch (rewardItem.RewardType) {
                case RewardType.Currency:
                    _walletService.Add(EnumExt.ValueOf<Currency>(rewardItem.RewardId), rewardItem.Count);
                    break;
                default:
                    throw new ArgumentException($"RewardType not found, type:= {rewardItem.RewardType}");
            }
        }
        public void ApplyRewards(IEnumerable<RewardItem> items)
        {
            foreach (var rewardItem in items)
            {
                ApplyReward(rewardItem);
            }
        }
    }
}
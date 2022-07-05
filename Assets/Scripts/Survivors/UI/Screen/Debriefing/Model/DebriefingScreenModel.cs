using System.Collections.Generic;
using System.Linq;
using Survivors.Player.Wallet;
using Survivors.Reward.Model;
using Survivors.Session.Model;

namespace Survivors.UI.Screen.Debriefing.Model
{
    public class DebriefingScreenModel
    {
        public readonly SessionResult SessionResult;
        public readonly Session.Model.Session Session;

        public DebriefingScreenModel(SessionResult sessionResult, Session.Model.Session session)
        {
            SessionResult = sessionResult;
            Session = session;
        }

        public ResultPanelModel BuildResultPanelModel(List<RewardItem> rewards, int levelNumber)
        {
            var coinsCount = rewards.First(it => it.RewardId == Currency.Soft.ToString()).Count;
            return new ResultPanelModel
            {
                SessionResult = SessionResult,
                KillCount = Session.Kills,
                CoinsCount = coinsCount,
                CurrentLevel = levelNumber
            };
        }
    }
}
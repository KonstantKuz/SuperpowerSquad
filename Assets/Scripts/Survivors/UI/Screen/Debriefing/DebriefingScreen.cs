using System.Collections.Generic;
using System.Linq;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.Player.Wallet;
using Survivors.Reward.Model;
using Survivors.Advertisment.Service;
using Survivors.Reward.Service;
using Survivors.UI.Screen.Main;
using Survivors.UI.Screen.Debriefing.Model;
using Survivors.UI.Screen.Menu;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Debriefing;
        public override ScreenId ScreenId => ID; 
        
        public static readonly string URL = MenuScreen.ID + "/" + ID;
        public override string Url => URL;

        [SerializeField]
        private SessionResultPanel _resultPanel;
        [SerializeField]
        private RewardMultiplier _rewardMultiplier;

        private DebriefingScreenModel _model;
        private RewardMultiplierModel _multiplierModel;
        
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        [Inject]
        private MissionResultRewardService _missionResultRewardService;  
        [Inject]
        private IRewardApplyService _rewardApplyService;
        [Inject]
        private AdsManager _adsManager;
        
        [PublicAPI]
        public void Init(DebriefingScreenModel model)
        {
            _model = model;
            
            InitRewardMultiplier();
            InitResultPanel();
        }

        private void InitRewardMultiplier()
        {
            var coinsReward = GetLevelRewards().First(IsCoinsReward).Count;
            _multiplierModel = new RewardMultiplierModel(coinsReward, MultiplyRewardByAds, DeclineMultiply);
            _rewardMultiplier.Init(_multiplierModel);
        }

        private void InitResultPanel()
        {
            var resultPanelModel = _model.BuildResultPanelModel(GetLevelRewards());
            _resultPanel.Init(resultPanelModel);
        }

        private void MultiplyRewardByAds()
        {
            _multiplierModel.Stop();
            if (_adsManager.IsRewardAdsReady()) {
                _adsManager.ShowRewardedAds(OnShownRewarded);
            } else {
                DeclineMultiply();
                this.Logger().Warn($"Reward not ready, place:= {nameof(DebriefingScreen)}");
            }
        }
        private void OnShownRewarded(bool success)
        {
            if (success) {
                AcceptMultiply(_multiplierModel.MultiplierValue.Value);
                this.Logger().Info($"Rewarded ad is successful, place:= {nameof(DebriefingScreen)}");
            } else {
                DeclineMultiply();
                this.Logger().Warn($"Rewarded ad failed, place:= {nameof(DebriefingScreen)}");
            }
        }

        private void AcceptMultiply(int multiplier)
        {
            _rewardApplyService.ApplyRewards(GetLevelRewards(multiplier));
            OnExit();
        }

        private void DeclineMultiply() => AcceptMultiply(1);

        private List<RewardItem> GetLevelRewards(int multiplier = 1)
        {
            return _missionResultRewardService.CalculateRewards(_model.SessionResult, _model.Session, multiplier);
        }

        private bool IsCoinsReward(RewardItem rewardItem)
        {
            return rewardItem.RewardId == Currency.Soft.ToString();
        }

        private void OnExit()
        {
            _adsManager.ShowInterstitialAds(() => _screenSwitcher.SwitchTo(MainScreen.URL));            
        }

        private void Update()
        {
            _multiplierModel.UpdateMultiplierValue();
        }
    }
}
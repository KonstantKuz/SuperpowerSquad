using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using Logger.Extension;
using Survivors.Location;
using Survivors.Session.Service;
using UnityEngine;
using Zenject;
using AdsManager = Survivors.Advertisment.Service.AdsManager;

namespace Survivors.UI.Dialog.ReviveDialog
{
    public class ReviveDialog : BaseDialog
    {
        [SerializeField] private ActionButton _reviveButton;
        [SerializeField] private ActionButton _restartButton;
        
        [Inject] private World _world;
        [Inject] private ReviveService _reviveService;     
        [Inject] private AdsManager _adsManager;

        private void Awake()
        {
            _reviveButton.Init(Revive);
            _restartButton.Init(Restart);
        }

        private void OnEnable()
        {
            _reviveButton.Button.interactable = true;
            _world.Pause();
        }

        private void OnDisable()
        {
            _world.UnPause();
        }

        private void Revive()
        {
            if (_adsManager.IsRewardAdsReady()) {
                _reviveButton.Button.interactable = false;
                _adsManager.ShowRewardedAds(OnShownRewarded);
            } else {
                this.Logger().Warn($"Reward not ready, place:= {nameof(ReviveDialog)}");
            }
        }

        private void OnShownRewarded(bool success)
        {
            _reviveButton.Button.interactable = true;
            if (success) {
                this.Logger().Info($"Rewarded ad is successful, place:= {nameof(ReviveDialog)}");
                _reviveService.Revive();
                Hide();
            } else {
                this.Logger().Warn($"Rewarded ad failed, place:= {nameof(ReviveDialog)}");
            }
        }

        private void Restart()
        {
            _world.Squad.Kill();
            Hide();
        }
    }
}
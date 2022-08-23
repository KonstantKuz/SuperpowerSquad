using System;
using Logger.Extension;
using Survivors.Advertisment.Providers;
using Zenject;

namespace Survivors.Advertisment.Service
{
    public class AdsManager
    {
        [Inject]
        private IAdsProvider _adsProvider;
        public IAdsProvider AdsProvider 
        { 
            get => _adsProvider;
            set
            {
                _adsProvider = value;
                this.Logger().Info($"IAdsProvider changed, new AdsProvider is {_adsProvider.GetType().Name}");
            }
        }
        public bool IsRewardAdsReady()
        {
            return AdsProvider.IsRewardAdsReady();
        }
        public bool ShowRewardedAds(Action<bool> success)
        {
            return AdsProvider.ShowRewardedAds(success);
        }
        public bool ShowInterstitialAds(Action action, float delay = -1f, bool force = false)
        {
            return AdsProvider.ShowInterstitialAds(action, delay, force);
        }
    }
}
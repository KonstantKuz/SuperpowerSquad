using System;
using Survivors.Advertisment.Providers;

namespace Survivors.Advertisment.Service
{
    public class AdsManager
    {
        private readonly IAdsProvider _adsProvider;

        public AdsManager(IAdsProvider adsProvider)
        {
            _adsProvider = adsProvider;
        }
        public bool IsRewardAdsReady()
        {
            return _adsProvider.IsRewardAdsReady();
        }
        public bool ShowRewardedAds(Action<bool> success)
        {
            return _adsProvider.ShowRewardedAds(success);
        }
        public bool ShowInterstitialAds(Action action, float delay = -1f, bool force = false)
        {
            return _adsProvider.ShowInterstitialAds(action, delay, force);
        }
    }
}
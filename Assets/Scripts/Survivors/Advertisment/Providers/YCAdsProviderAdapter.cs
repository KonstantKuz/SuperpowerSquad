using System;
using YsoCorp.GameUtils;

namespace Survivors.Advertisment.Providers
{
    public class YCAdsProviderAdapter : IAdsProvider
    {
        private AdsManager AdManager => YCManager.instance.adsManager;
        
        public bool IsRewardAdsReady()
        { 
            return AdManager.IsRewardBasedVideo();
        }
        public bool ShowRewardedAds(Action<bool> success)
        {
            return AdManager.ShowRewarded(success);
        }
        public bool ShowInterstitialAds(Action action, float delay = -1f, bool force = false)
        { 
            return AdManager.ShowInterstitial(action, delay, force);
        }
    }
}
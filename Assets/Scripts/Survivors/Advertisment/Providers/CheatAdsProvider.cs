using System;

namespace Survivors.Advertisment.Providers
{
    public class CheatAdsProvider : IAdsProvider
    {
        public bool IsRewardAdsReady()
        {
            return true;
        }
        public bool ShowRewardedAds(Action<bool> success)
        {
            success?.Invoke(true);
            return true;
        }
        public bool ShowInterstitialAds(Action action, float delay = -1f, bool force = false)
        { 
            action?.Invoke();
            return true;
        }
    }
}
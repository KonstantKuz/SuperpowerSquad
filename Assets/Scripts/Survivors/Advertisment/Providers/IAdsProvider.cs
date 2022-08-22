using System;

namespace Survivors.Advertisment.Providers
{
    public interface IAdsProvider
    {
        bool IsRewardAdsReady();
        bool ShowRewardedAds(Action<bool> success);
        bool ShowInterstitialAds(Action action, float delay, bool force = false);
    }
}
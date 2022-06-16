using Survivors.Analytics.Wrapper;
using Zenject;

namespace Survivors.Analytics
{
    public class AnalyticsInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<Analytics>()
                .FromNew()
                .AsSingle()
                .WithArguments(new IAnalyticsImpl[]
                {
                    new AppMetricaAnalyticsWrapper(),
                    new AppsFlyerAnalyticsWrapper(),
                    //TODO: not a good decision - will cause bugs in FacebookAnalyticsWrapper that are only reproduced on android/ios
                    //and not in Editor
#if !UNITY_EDITOR && !PLATFORM_STANDALONE
                            new FacebookAnalyticsWrapper()
#endif
                }).NonLazy();
            
        }
    }
}
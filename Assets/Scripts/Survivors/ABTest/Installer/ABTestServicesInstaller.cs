using Survivors.ABTest.Providers;
using Zenject;

namespace Survivors.ABTest.Installer
{
    public class ABTestServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<ABTest>().AsSingle();
            container.Bind<IABTestProvider>().To<OverrideABTestProvider>().AsSingle().WithArguments(new YCABTestProvider());
        }
    }
}
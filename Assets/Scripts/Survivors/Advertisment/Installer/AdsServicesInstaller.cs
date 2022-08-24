using Survivors.Advertisment.Providers;
using Survivors.Advertisment.Service;
using Zenject;

namespace Survivors.Advertisment.Installer
{
    public class AdsServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<AdsEventHandler>().AsSingle().NonLazy();
            container.Bind<IAdsProvider>().To<YCAdsProviderAdapter>().AsSingle();
            container.Bind<AdsManager>().AsSingle();
        }
    }
}
using Survivors.Location.Service;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;

namespace Survivors.Units.Installer
{
    public class UnitServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<TargetService>().AsSingle();  
            container.Bind<LocationObjectFactory>().AsSingle();       
            container.Bind<UnitFactory>().AsSingle();
        }
    }
}
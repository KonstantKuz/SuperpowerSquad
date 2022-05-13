using Survivors.Location.Service;
using Survivors.Units.Service;
using Survivors.Units.Target;
using Zenject;

namespace Survivors.Units.Installer
{
    public class UnitServicesInstaller
    {
        public static void Install(DiContainer container, Location.Location location)
        {
            container.Bind<TargetService>().AsSingle();  
            container.Bind<LocationObjectFactory>().AsSingle();       
            container.Bind<UnitFactory>().AsSingle();
            container.Bind<Location.Location>().FromInstance(location).AsSingle();
        }
    }
}
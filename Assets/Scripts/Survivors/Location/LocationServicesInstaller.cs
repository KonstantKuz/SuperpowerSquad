using Survivors.Location.Service;
using Survivors.Session;
using Zenject;

namespace Survivors.Location
{
    public class LocationServicesInstaller
    {
        public static void Install(DiContainer container, 
            LocationObjectFactory locationObjectFactory, 
            LocationWorld locationWorld)
        {
            locationObjectFactory.Init();
            container.Bind<LocationObjectFactory>().FromInstance(locationObjectFactory).AsSingle();
            container.Bind<LocationWorld>().FromInstance(locationWorld);
            container.Bind<SessionService>().AsSingle();
        }
    }
}
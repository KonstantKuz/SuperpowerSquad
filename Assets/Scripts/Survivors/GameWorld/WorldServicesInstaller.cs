using Survivors.GameWorld.Service;
using UnityEngine;
using Zenject;

namespace Survivors.GameWorld
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private WorldObjectFactory _worldObjectFactory;
        
        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.Bind<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<World>().FromInstance(_world);
        }
    }

}
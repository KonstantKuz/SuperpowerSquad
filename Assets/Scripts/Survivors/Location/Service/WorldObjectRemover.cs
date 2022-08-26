using Survivors.Location.ObjectFactory;
using Zenject;

namespace Survivors.Location.Service
{
    public class WorldObjectRemover : IWorldScope
    {
        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectInstancingFactory;
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _objectPoolFactory;
        public void OnWorldSetup() { }
        public void OnWorldCleanUp()
        {
            _objectPoolFactory.DestroyAllObjects();
            _objectInstancingFactory.DestroyAllObjects();
        }
    }
}
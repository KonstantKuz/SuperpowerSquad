using Feofun.App.Init;
using JetBrains.Annotations;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject]
        private UnitFactory _unitFactory;
        public StartGameInitStep()
        {
        }

        protected override void Run()
        {
            _unitFactory.LoadPlayerUnit();
            Next();
        }
    }
}
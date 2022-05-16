using Feofun.App.Init;
using JetBrains.Annotations;
using Survivors.Session;
using Zenject;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject] private SessionService _sessionService;
        
        public StartGameInitStep()
        {
        }

        protected override void Run()
        {
            _sessionService.Start();
            Next();
        }
    }
}
using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Logger.Assets.Scripts;
using Survivors.UI.Screen.World;
using Survivors.Units.Enemy;
using Zenject;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger<StartGameInitStep>();
        
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        
        protected override void Run()
        {
            _logger.Error("Run startGameInitStep");
            NavMeshInitializer.Init();
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
            Next();
        }
    }
}
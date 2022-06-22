using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Logger.Assets.Scripts.Extension;
using Survivors.UI.Screen.World;
using Survivors.Units.Enemy;
using Zenject;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        
        protected override void Run()
        {
            NavMeshInitializer.Init();
            this.Logger().Debug("INIT");
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
            Next();
        }
    }
}
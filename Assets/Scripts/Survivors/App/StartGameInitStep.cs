using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.UI.Screen.World;
using Zenject;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        
        
        public StartGameInitStep()
        {
        }

        protected override void Run()
        {
            _screenSwitcher.SwitchTo(WorldScreen.WORLD_SCREEN.ToString());
            Next();
        }
    }
}
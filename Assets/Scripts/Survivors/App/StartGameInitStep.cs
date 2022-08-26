using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.UI.Screen.Main;
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
        [Inject(Id = ObjectFactoryType.Pool)] 
        protected IObjectFactory _objectFactory;  
        protected override void Run()
        {
            DOTweenInitializer.Init();
            NavMeshInitializer.Init();
            _objectFactory.Prepare();
            _screenSwitcher.SwitchTo(MainScreen.URL);
            Next();
        }
    }
}
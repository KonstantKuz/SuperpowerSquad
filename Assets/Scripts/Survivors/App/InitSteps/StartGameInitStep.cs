using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.ObjectPool.Service;
using Survivors.UI.Screen.Main;
using Survivors.Units.Enemy;
using Zenject;

namespace Survivors.App.InitSteps
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        [Inject] 
        private PoolPreparer _poolPreparer;  
        
        protected override void Run()
        {
            DOTweenInitializer.Init();
            NavMeshInitializer.Init();
            _poolPreparer.Prepare();
            _screenSwitcher.SwitchTo(MainScreen.URL);
            Next();
        }
    }
}
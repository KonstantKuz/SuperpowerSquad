using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.UI.Screen.World;
using Survivors.Units.Enemy;
using UnityEngine;
using UnityEngine.AI;
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
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
            Next();
        }
    }
}
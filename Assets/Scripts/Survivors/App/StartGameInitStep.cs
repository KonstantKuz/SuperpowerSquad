using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.UI.Screen.World;
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
            InitNavMesh();
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
            Next();
        }

        private void InitNavMesh()
        {
            NavMesh.avoidancePredictionTime = 0.5f;
            NavMesh.pathfindingIterationsPerFrame = 500;
        }
    }
}
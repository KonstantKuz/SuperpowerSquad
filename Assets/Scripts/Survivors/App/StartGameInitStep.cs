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
        private const float AVOIDANCE_PREDICTION_TIME = 0.5f;
        private const int PATHFINDING_ITERATIONS_PER_FRAME = 500;

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
            NavMesh.avoidancePredictionTime = AVOIDANCE_PREDICTION_TIME;
            NavMesh.pathfindingIterationsPerFrame = PATHFINDING_ITERATIONS_PER_FRAME;
        }
    }
}
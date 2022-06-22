using System;
using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Logger.Assets.Scripts;
using Survivors.UI.Screen.World;
using Survivors.Units.Enemy;
using UnityEngine;
using Zenject;
using ILogger = Logger.Assets.Scripts.ILogger;

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
            _logger.Info("Init game", new ArgumentException("MY EXP"));
            Debug.LogException(new ArgumentException("MY EXP"));
            NavMeshInitializer.Init();
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
            Next();
        }
    }
}
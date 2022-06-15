using Feofun.App;
using Feofun.App.Init;
using JetBrains.Annotations;
using Survivors.Analytics;
using Survivors.App;
using Survivors.IOSTransparency;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Survivors
{
    public class GameApplication : MonoBehaviour
    {
        [PublicAPI]
        public static GameApplication Instance { get; private set; }

        [Inject]
        public DiContainer Container;

        private void Awake()
        {
            Instance = this;
            AppContext.Container = Container;

#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += HandleOnPlayModeChanged;
            void HandleOnPlayModeChanged(PauseState pauseState)
            {
                
            }
#endif
            DontDestroyOnLoad(gameObject);
            RunLoadableChains();
        }
        private void RunLoadableChains()
        {
            var initSequence = gameObject.AddComponent<AppInitSequence>();
           
            initSequence.AddStep<IosATTInitStep>();
            initSequence.AddStep<AnalyticsInitStep>();
            initSequence.AddStep<StartGameInitStep>();
            initSequence.Next();
        }
    }
}
using Feofun.App;
using Feofun.App.Init;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.ABTest.InitStep;
using Survivors.Analytics;
using Survivors.App.InitSteps;
#if UNITY_IOS
using Survivors.IOSTransparency;
#endif
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Survivors
{
    public class GameApplication : MonoBehaviour, ICoroutineRunner
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

#if UNITY_IOS
            initSequence.AddStep<IosATTInitStep>();            
#endif
            initSequence.AddStep<YsoCorpSDKInitStep>(new []{this});
            initSequence.AddStep<ABTestInitStep>();
            initSequence.AddStep<AnalyticsInitStep>();
            initSequence.AddStep<StartGameInitStep>();
            initSequence.Next();
        }
    }
}
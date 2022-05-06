using Feofun.App;
using Feofun.App.Loadable;
using JetBrains.Annotations;
using Survivors.App;
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
            var loadableChain = gameObject.AddComponent<AppLoadableChain>();
            loadableChain.AddLoadable<StartGameLoadable>();
            loadableChain.Next();
        }
    }
}
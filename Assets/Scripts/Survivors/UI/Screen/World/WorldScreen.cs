using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Session;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId WORLD_SCREEN = ScreenId.World;
        
        public override ScreenId ScreenId => WORLD_SCREEN;
        public override string Url => ScreenName;
        
        [Inject] private SessionService _sessionService;
        
        [PublicAPI]
        public void Init()
        {
            _sessionService.Start();
        }

        private void OnDisable()
        {
            _sessionService.Term();
        }
    }
}
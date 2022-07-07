using System.Collections;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Session.Service;
using Survivors.UI.Screen.Menu;
using Survivors.UI.Screen.World;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Zenject;

namespace Survivors.UI.Screen.Main
{
    public class MainScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Main;
        public override ScreenId ScreenId => ID;
        public static readonly string URL = MenuScreen.ID + "/" + ID;
        public override string Url => URL;

        [SerializeField] private LevelProgressBar _levelProgressBar;
        
        private CompositeDisposable _disposable;

        [Inject] private SessionService _sessionService;
        [Inject] private ScreenSwitcher _screenSwitcher;     
        [Inject] private Location.World _world;
        [Inject] private Joystick _joystick;

        [PublicAPI]
        public void Init()
        {
            Assert.IsNull(_disposable);
            _disposable = new CompositeDisposable();
            
            _world.Setup();
            _sessionService.Start();
        
            _joystick.Attach(transform); // todo: can be replaced by adding to the parent screen
            _levelProgressBar.Init(_sessionService.LevelConfig.Level);
            
            StartCoroutine(WaitForAnimationUpdateBeforePause());
        }
        
        private IEnumerator WaitForAnimationUpdateBeforePause()
        {
            yield return new WaitForEndOfFrame();
            _world.Pause();
            WaitForClickOrDrag();
        }

        private void WaitForClickOrDrag()
        {
            var startGameArea = _joystick.GetComponent<UIBehaviour>();
            var areaDrag = startGameArea.OnDragAsObservable();
            var areaClick = startGameArea.OnPointerClickAsObservable();
            areaDrag.Merge(areaClick).First().Subscribe(StartSession).AddTo(_disposable);
        }

        private void StartSession(PointerEventData eventData)
        {
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
        }

        public override IEnumerator Hide()
        {
            Dispose();
            return base.Hide();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}

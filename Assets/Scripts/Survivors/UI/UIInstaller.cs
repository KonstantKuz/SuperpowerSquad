using Feofun.UI;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace Survivors.UI
{
    public class UIInstaller : MonoBehaviour
    {
        [SerializeField] 
        private UIRoot _uiRoot;
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private ScreenSwitcher _screenSwitcher;

        public void Install(DiContainer container)
        {
            container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            container.Bind<Joystick>().FromInstance(_joystick).AsSingle();
            container.Bind<ScreenSwitcher>().FromInstance(_screenSwitcher).AsSingle();
        }
    }
}
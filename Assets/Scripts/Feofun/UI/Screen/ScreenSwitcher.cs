using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using UnityEngine;

namespace Feofun.UI.Screen
{
    public class ScreenSwitcher : MonoBehaviour
    {
        [SerializeField] 
        private List<BaseScreen> _screens;
        
        public BaseScreen ActiveScreen { get; private set; }
        
        public event Action<string> OnAfterScreenSwitched;

        private void Awake()
        {
            DeActivateAll();
        }
        public void SwitchTo(string url, bool async = false, params object[] initParams)
        {
            var path = url.Split('/');
            var screenName = path.Last();
            var switchingParams = new SwitchingParam(async).SetParamForScreen(screenName, initParams);
            SwitchTo(url, switchingParams);
        }
        public void SwitchTo(string url, SwitchingParam switchingParam)
        {
            var coroutine = ScreenSwitchCoroutine(url, switchingParam);
            if (!switchingParam.Async) {
                coroutine.RunBlocking();
            } else {
                StartCoroutine(coroutine);
            }
        }
        public IEnumerator HideActiveScreen()
        {
            if (ActiveScreen == null) yield break;
            yield return ActiveScreen.Hide();
            ActiveScreen = null;
        }
        private IEnumerator ScreenSwitchCoroutine(string url, SwitchingParam switchingParam)
        {
            var path = url.Split('/');
            var screenName = path[0];

            yield return SwitchScreen(screenName, switchingParam.FindParamForScreen(screenName));
            
            if (path.Length > 1) {
                yield return SwitchSubscreen(GetSubscreenUrl(path), switchingParam);
            }
            OnAfterScreenSwitched?.Invoke(url);
        }
        private IEnumerator SwitchSubscreen(string url, SwitchingParam switchingParam)
        {
            var childScreenSwitcher = ActiveScreen.GetComponent<ScreenSwitcher>();
            if (childScreenSwitcher == null)
            {
                throw new ArgumentException($"Active screen {ActiveScreen.ScreenName} do not have ScreenSwitcher, cannot go to {url}");
            }
            yield return childScreenSwitcher.ScreenSwitchCoroutine(url, switchingParam);
        }

        private IEnumerator SwitchScreen(string screenName, params object[] initParams)
        {
            var screen = GetScreen(screenName) ?? throw new ArgumentException($"Trying to switch to non-existing screen {screenName}");

            if (IsNotActiveScreen(screenName)) {
                yield return HideActiveScreen();
                ActiveScreen = screen;
                ActiveScreen.Show(initParams);
            } else {
                screen.CallScreenInit(initParams);
            }
        }
        private static string GetSubscreenUrl(string[] path) => string.Join("/", path.Skip(1));
        private bool IsNotActiveScreen(string screenName) => ActiveScreen == null || screenName != ActiveScreen.ScreenName;
        private void DeActivateAll() => _screens.ForEach(screen => screen.DeActivate());
        private BaseScreen GetScreen(string screenName) => _screens.FirstOrDefault(it => it.ScreenName == screenName);
    }
}
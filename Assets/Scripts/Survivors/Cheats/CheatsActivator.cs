using System;
using System.Globalization;
using UniRx;
using UnityEngine;

namespace Survivors.Cheats
{
    public class CheatsActivator : MonoBehaviour
    {
        private const string ENABLED_CHEAT_KEY = "Vkl_chit";
        private static string _pin = "191373";
        private static string _inputCode = "112233";
        
        [SerializeField] private GameObject _codeInputScreen;
        [SerializeField] private GameObject _cheatsButtonScreen;
        [SerializeField] private GameObject _cheatsPanelScreen;
        
        private string _enteredPin = string.Empty;
        private Vector2Int _virtualPinPadSize = new Vector2Int(3, 3);
        private Vector2 _cellSize;
        
        private bool _activated;

        private void Awake()
        {
            _cellSize = new Vector2(Screen.width / (float) _virtualPinPadSize.x, Screen.height / (float) _virtualPinPadSize.y);
            _activated = (PlayerPrefs.GetInt(ENABLED_CHEAT_KEY) != 0);
            if (_activated) {
                ShowCheatsButtonScreen(true);
            }
        }
        private void Update()
        {
            if (!_activated) {
                CheckPin();
            }
        }
        public bool IsValidInputCode(string inputCode) => inputCode == _inputCode;

        public void ShowCodeInputScreen(bool show) => _codeInputScreen.SetActive(show);

        public void ShowCheatsButtonScreen(bool show) => _cheatsButtonScreen.SetActive(show);
        
        public void ShowCheatsPanelScreen(bool show)
        {
            _cheatsPanelScreen.SetActive(show);
            Time.timeScale = show ? 0 : 1;
        }

        public void EnableCheats(bool enabled)
        {
            _activated = enabled;
            PlayerPrefs.SetInt(ENABLED_CHEAT_KEY, (_activated ? 1 : 0));
        }
        private void CheckPin()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            var xpos = Mathf.Floor(Input.mousePosition.x / _cellSize.x);
            var ypos = Mathf.Floor(Input.mousePosition.y / _cellSize.y);

            var buttonId = ypos * _virtualPinPadSize.x + xpos + 1;
            
            _enteredPin += buttonId.ToString(CultureInfo.InvariantCulture);
            
            if (_enteredPin.Length == 1) Observable.Timer(TimeSpan.FromSeconds(6)).Subscribe(it => _enteredPin = "");

            if (_enteredPin != _pin) return;
            ShowCodeInputScreen(true);
            _enteredPin = string.Empty;
        }
    }
}

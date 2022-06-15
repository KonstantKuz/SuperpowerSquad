using System;
using Survivors.Cheats.Repository;
using UnityEngine;
using Zenject;

namespace Survivors.Cheats
{
    public class CheatsManager : MonoBehaviour
    {
        
        private readonly CheatRepository _repository = new CheatRepository();
        
        [Inject] private DiContainer _container;

        [SerializeField] private GameObject _fpsMonitor;
        [SerializeField] private GameObject _debugConsole;

 
        private CheatSettings Settings => _repository.Get() ?? new CheatSettings();
        
        private void Awake()
        {
            _debugConsole.SetActive(IsConsoleEnabled);
#if DEBUG_CONSOLE_ENABLED
            _debugConsole.SetActive(true);
#endif
        }
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
        }
        
        public void AddExp(int amount)
        {
            _progressService.AddExp(amount);
        }
        
        public void ToggleFPSMonitor() => _fpsMonitor.SetActive(!_fpsMonitor.activeInHierarchy);

        public void ToggleDebugConsole() => _debugConsole.SetActive(!_debugConsole.activeInHierarchy);
        
        public void SetLanguage(string language)
        {
            _localizationService.SetLanguageOverride(language);
        }
        
        public bool IsConsoleEnabled
        {
            get => Settings.ConsoleEnabled;
            set
            {
                UpdateSettings(settings => { settings.ConsoleEnabled = value; });
            }
        }
        private void UpdateSettings(Action<CheatSettings> updateFunc)
        {
            var settings = Settings;
            updateFunc?.Invoke(settings);
            _repository.Set(settings);
        }
    }
}


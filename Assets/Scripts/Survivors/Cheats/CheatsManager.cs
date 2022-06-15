using System;
using Feofun.Localization.Service;
using Survivors.Cheats.Repository;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using UnityEngine;
using Zenject;

namespace Survivors.Cheats
{
    public class CheatsManager : MonoBehaviour
    {
        private readonly CheatRepository _repository = new CheatRepository();
        
        [Inject] private LocalizationService _localizationService;     
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private UpgradeService _upgradeService;

        [SerializeField] private GameObject _fpsMonitor;
        [SerializeField] private GameObject _debugConsole;
        
        private CheatSettings Settings => _repository.Get() ?? new CheatSettings();
        
        private void Awake()
        {
#if DEBUG_CONSOLE_ENABLED
            IsConsoleEnabled = true;
#endif
            _debugConsole.SetActive(IsConsoleEnabled); 
            _fpsMonitor.SetActive(IsFPSMonitorEnabled);

        }
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
        
        public void IncreaseSquadLevel() => _squadProgressService.IncreaseLevel();
        private void AddRandomSquadUpgrade() => _upgradeService.AddRandomUpgrade();
     
        public void SetLanguage(string language)
        {
            _localizationService.SetLanguageOverride(language);
        }
        public void ToggleDebugConsole()
        {
            IsConsoleEnabled = !isActiveAndEnabled;
            _debugConsole.SetActive(IsConsoleEnabled);
        }
        public void ToggleFPSMonitor()
        {
            IsFPSMonitorEnabled = !IsFPSMonitorEnabled;
            _fpsMonitor.SetActive(IsFPSMonitorEnabled);
        }
        private void UpdateSettings(Action<CheatSettings> updateFunc)
        {
            var settings = Settings;
            updateFunc?.Invoke(settings);
            _repository.Set(settings);
        }
        private bool IsConsoleEnabled
        {
            get => Settings.ConsoleEnabled;
            set
            {
                UpdateSettings(settings => { settings.ConsoleEnabled = value; });
            }
        }    
        private bool IsFPSMonitorEnabled
        {
            get => Settings.FPSMonitorEnabled;
            set
            {
                UpdateSettings(settings => { settings.FPSMonitorEnabled = value; });
            }
        }
    }
}


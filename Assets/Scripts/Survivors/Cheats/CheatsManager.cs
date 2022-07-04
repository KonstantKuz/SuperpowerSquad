using System;
using Feofun.Localization.Service;
using Survivors.Cheats.Data;
using Survivors.Cheats.Repository;
using Survivors.Player.Inventory.Service;
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
        [Inject] private InventoryService _inventoryService;

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
        public void AddRandomSquadUpgrade() => _upgradeService.AddRandomUpgrade();   
        public void ApplyAllSquadUpgrades() => _upgradeService.ApplyAllUpgrades();  
        public void AddUnit(string unitId) => _upgradeService.AddUnit(unitId);
        public void AddMetaUpgrade(string upgradeId) => _inventoryService.AddUpgrade(upgradeId);
        
        public void SetLanguage(string language)
        {
            _localizationService.SetLanguageOverride(language);
        }
        private void UpdateSettings(Action<CheatSettings> updateFunc)
        {
            var settings = Settings;
            updateFunc?.Invoke(settings);
            _repository.Set(settings);
        }
        public bool IsConsoleEnabled
        {
            get => Settings.ConsoleEnabled;
            set
            {
                UpdateSettings(settings => { settings.ConsoleEnabled = value; });
                _debugConsole.SetActive(value);
            }
        }    
        public bool IsFPSMonitorEnabled
        {
            get => Settings.FPSMonitorEnabled;
            set
            {
                UpdateSettings(settings => { settings.FPSMonitorEnabled = value; });
                _fpsMonitor.SetActive(value);
            }
        }
    }
}


using Feofun.UI.Components.Button;
using Survivors.Cheats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Cheats
{
    public class CheatsScreenPresenter : MonoBehaviour
    {
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _hideButton;

        [SerializeField] private ActionToggle _toggleConsoleButton;
        [SerializeField] private ActionToggle _toggleFPSButton;

        [SerializeField] private ActionButton _increaseSquadLevelButton;
        [SerializeField] private ActionButton _addRandomSquadUpgrade; 
        [SerializeField] private ActionButton _resetProgressButton;

        [SerializeField] private InputField _inputField;
        [SerializeField] private ActionButton _setLanguage;

        [SerializeField] private ActionButton _setRussianLanguage;
        [SerializeField] private ActionButton _setEnglishLanguage;

        [Inject] private CheatsManager _cheatsManager;
        [Inject] private CheatsActivator _cheatsActivator;

        private void OnEnable()
        {
            _closeButton.Init(HideCheatsScreen);
            _hideButton.Init(DisableCheats);

            _toggleConsoleButton.Init(_cheatsManager.IsConsoleEnabled, value => { _cheatsManager.IsConsoleEnabled = value; });
            _toggleFPSButton.Init(_cheatsManager.IsFPSMonitorEnabled, value => { _cheatsManager.IsFPSMonitorEnabled = value; });
          
            _increaseSquadLevelButton.Init(_cheatsManager.IncreaseSquadLevel);     
            _addRandomSquadUpgrade.Init(_cheatsManager.AddRandomSquadUpgrade);
            _resetProgressButton.Init(_cheatsManager.ResetProgress);
            
            _setLanguage.Init(() => _cheatsManager.SetLanguage(_inputField.text));
            _setEnglishLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.English.ToString()));
            _setRussianLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.Russian.ToString()));
        }
        private void DisableCheats()
        {
            _cheatsActivator.ShowOpenCheatButton(false);
            _cheatsActivator.EnableCheats(false);
            HideCheatsScreen();
        }
        private void HideCheatsScreen() => _cheatsActivator.ShowCheatsScreen(false);
    }
}
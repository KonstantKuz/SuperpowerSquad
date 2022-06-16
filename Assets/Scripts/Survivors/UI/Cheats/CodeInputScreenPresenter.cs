using Feofun.Cheats;
using Feofun.UI.Components.Button;
using Survivors.Cheats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Cheats
{
    public class CodeInputScreenPresenter : MonoBehaviour
    {
        [Inject] private CheatsActivator _cheatsActivator;

        [SerializeField] private InputField _inputField;
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _confirmButton;

        private void OnEnable()
        {
            _closeButton.Init(HideCodeInputPanel);
            _confirmButton.Init(CheckInputCode);
        }

        private void CheckInputCode()
        {
            if (_cheatsActivator.IsValidInputCode(_inputField.text))
            {
                ShowOpenCheatButton();
                HideCodeInputPanel();
                EnableCheats();
            }
        }
        private void HideCodeInputPanel() => _cheatsActivator.ShowCodeInputPanel(false);
        private void EnableCheats() => _cheatsActivator.EnableCheats(true);
        private void ShowOpenCheatButton() => _cheatsActivator.ShowOpenCheatButton(true);
    }
}
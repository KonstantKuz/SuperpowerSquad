using Feofun.Cheats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Feofun.UI.Cheats
{
    public class OpenCheatsButtonPresenter : MonoBehaviour
    {
        [Inject] private CheatsActivator _cheatsActivator;
        
        [SerializeField] private Button _openButton;

        private void OnEnable() => _openButton.onClick.AddListener(ShowCheatsScreen);

        private void OnDisable() => _openButton.onClick.RemoveAllListeners();

        private void ShowCheatsScreen() => _cheatsActivator.ShowCheatsScreen(true);
    }
}
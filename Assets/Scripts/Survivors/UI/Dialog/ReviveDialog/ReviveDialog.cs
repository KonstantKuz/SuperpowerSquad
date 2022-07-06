using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using Survivors.Location;
using Survivors.Session.Service;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.ReviveDialog
{
    public class ReviveDialog : BaseDialog
    {
        [SerializeField] private ActionButton _reviveButton;
        [SerializeField] private ActionButton _restartButton;
        
        [Inject] private World _world;
        [Inject] private ReviveService _reviveService;

        private void Awake()
        {
            _reviveButton.Init(Revive);
            _restartButton.Init(Restart);
        }

        private void OnEnable()
        {
            _world.Pause();
        }

        private void OnDisable()
        {
            _world.UnPause();
        }

        private void Revive()
        {
            _reviveService.Revive();
            Hide();
        }

        private void Restart()
        {
            _world.Squad.Kill();
            Hide();
        }
    }
}
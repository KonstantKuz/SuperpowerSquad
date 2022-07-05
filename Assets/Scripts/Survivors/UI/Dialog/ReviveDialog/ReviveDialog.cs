using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using SuperMaxim.Core.Extensions;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Units;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.ReviveDialog
{
    public class ReviveDialog : BaseDialog
    {
        [SerializeField] private ActionButton _reviveButton;
        [SerializeField] private ActionButton _restartButton;
        
        [Inject] private World _world;
        [Inject] private UnitService _unitService;
        [Inject] private ConstantsConfig _constantsConfig;

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
            _world.Squad.RestoreHealth();
            KillEnemiesAroundSquad();
            Hide();
        }

        private void Restart()
        {
            _world.Squad.Kill();
            Hide();
        }

        private void KillEnemiesAroundSquad()
        {
            _unitService.GetUnitsInRadius(_world.Squad.transform.position, UnitType.ENEMY,
                _constantsConfig.ReviveEnemyRemoveRadius)
                .ForEach(it => it.Kill(DeathCause.Removed));
        }
    }
}
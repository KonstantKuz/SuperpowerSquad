using System.Linq;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Player.Progress.Service;
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
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private PlayerProgressService _playerProgressService;

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
            _playerProgressService.AddRevive();
            _analytics.ReportRevive();
            Hide();
        }

        private void Restart()
        {
            _world.Squad.Kill();
            Hide();
        }

        private void KillEnemiesAroundSquad()
        {
            var enemiesNearby = _unitService.GetUnitsInRadius(_world.Squad.Position, UnitType.ENEMY,
                _constantsConfig.ReviveEnemyRemoveRadius).ToList();
            enemiesNearby.ForEach(it => it.Kill(DeathCause.Removed));
        }
    }
}
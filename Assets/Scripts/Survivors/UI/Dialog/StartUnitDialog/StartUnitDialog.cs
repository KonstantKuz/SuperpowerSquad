using System;
using Feofun.UI.Components;
using Feofun.UI.Dialog;
using Survivors.Squad.Upgrade.Config;
using Survivors.UI.Dialog.StartUnitDialog.Model;
using Survivors.UI.Dialog.StartUnitDialog.View;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.StartUnitDialog
{
    public class StartUnitDialog : BaseDialog, IUiInitializable<Action<StartUnitSelection>>
    {
        [SerializeField]
        private StartUnitView _view;
        
        [Inject] private UpgradesConfig _upgradesConfig;   
   

        private StartUnitDialogModel _model;

        public void Init(Action<StartUnitSelection> onApplyUnit)
        {
            _model = new StartUnitDialogModel(_upgradesConfig, onApplyUnit);
            _view.Init(_model);
        }
        
        private void OnDisable()
        {
            _model = null;
        }
        
    }
}
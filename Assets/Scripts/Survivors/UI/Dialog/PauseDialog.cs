using System;
using Feofun.UI.Dialog;
using Survivors.Location;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using Zenject;

namespace Survivors.UI.Dialog
{
    public class PauseDialog : BaseDialog
    {
        [Inject] private DialogManager _dialogManager;    
        [Inject] private World _world;      
        [Inject] private Joystick _joystick;
        
        private IDisposable _disposable;
        
        private void OnEnable()
        {
            Dispose();
            var uiBehaviour = _joystick.GetComponent<UIBehaviour>();
            _disposable = uiBehaviour.OnDragAsObservable().Merge(uiBehaviour.OnPointerClickAsObservable()).First().Subscribe(it => OnClick());
        }
        private void OnClick()
        {
            _dialogManager.Hide<PauseDialog>();
            _world.UnPause();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
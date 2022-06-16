using Feofun.UI.Components;
using Survivors.App.Config;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud.Unit
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarView _progressBar;  
        [SerializeField]
        private RectTransform _barContainer;
    
        [Inject]
        private ConstantsConfig _constantsConfig;
        
        private CompositeDisposable _disposable;
        private HealthBarModel _model;
 

        public void Init(HealthBarModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _model = model;
            model.Percent.Subscribe(UpdateProgressBar).AddTo(_disposable);
            model.MaxValue.Subscribe(UpdateMaxValue).AddTo(_disposable);
        }
        private void UpdateProgressBar(float value)
        {
            _progressBar.SetData(value);
        }
        private void UpdateMaxValue(float maxValue)
        {
            var scaleIncrementDelta = ((maxValue - _model.StartingMaxValue) * _constantsConfig.HealthScaleIncrementFactor) / _model.StartingMaxValue;
            var barLength = 1 + scaleIncrementDelta;
            var scale = _barContainer.localScale;
            _barContainer.localScale = new Vector3(barLength, scale.y, scale.z);
        }   

        protected void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }
    }
}
using Feofun.UI.Components;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Hud.Unit
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private ProgressBarView _progressBar;

        public void Init(HealthBarModel model)
        {
            model.Percent.Subscribe(UpdateBar);
        }
        
        private void UpdateBar(float value)
        {
            _progressBar.SetData(value);
        }
    }
}
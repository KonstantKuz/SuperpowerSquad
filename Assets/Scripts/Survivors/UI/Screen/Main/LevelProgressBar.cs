using Feofun.UI.Components;
using UnityEngine;

namespace Survivors.UI.Screen.Main
{
    public class LevelProgressBar : MonoBehaviour
    {
        [SerializeField] private float _levelStep;
        [SerializeField] private ProgressBarView _progressBarView;

        public void Init(int currentLevel)
        {
            _progressBarView.SetValueWithLoop(currentLevel + 1 / _levelStep);
        }
    }
}

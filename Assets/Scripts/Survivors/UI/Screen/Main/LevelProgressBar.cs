using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.UI.Components;
using UnityEngine;

namespace Survivors.UI.Screen.Main
{
    public class LevelProgressBar : MonoBehaviour
    {
        private const int MIN_LEVEL_NUMBER = 1;
        private const int MAX_LEVEL_NUMBER = 5;
        
        [SerializeField] private ProgressBarView _levelBar;
        [SerializeField] private List<ProgressByStep> _progressByStep;

        public void Init(float currentLevel)
        {
            _levelBar.IsIndependentUpdate = true;
            
            var step = Mathf.Clamp(currentLevel + 1, MIN_LEVEL_NUMBER, MAX_LEVEL_NUMBER);
            _levelBar.SetData(_progressByStep.First(it => it.Step == step).ProgressValue);
        }
        
        [Serializable]
        private struct ProgressByStep
        {
            public int Step;
            public float ProgressValue;
        }
    }
}

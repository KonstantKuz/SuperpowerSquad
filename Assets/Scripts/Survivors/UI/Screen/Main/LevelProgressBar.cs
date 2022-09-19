using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.UI.Components;
using Feofun.Util.SerializableDictionary;
using TMPro;
using UnityEngine;

namespace Survivors.UI.Screen.Main
{
    public class LevelProgressBar : MonoBehaviour
    {
        private const int MAX_STEP_NUMBER = 5;
        
        [SerializeField] private ProgressBarView _levelBar;
        [SerializeField] private List<TextMeshProUGUI> _levelLabels;
        [SerializeField] private List<float> _progressValues;

        public void Init(int value)
        {
            var currentStep = value % MAX_STEP_NUMBER;
            _levelBar.Reset(_progressValues[currentStep]);

            var initialDisplayedValue = value - currentStep + 1;
            FillLabels(initialDisplayedValue);
        }
        
        private void FillLabels(int initialValue) 
        {
            for (int labelIdx = 0; labelIdx < _levelLabels.Count; labelIdx++)
            {
                _levelLabels[labelIdx].SetText($"{initialValue + labelIdx}");
            }
        }
    }
}

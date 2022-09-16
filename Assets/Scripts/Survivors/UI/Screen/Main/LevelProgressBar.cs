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

        public void Init(int winCount)
        {
            var currentStep = winCount % MAX_STEP_NUMBER;
            _levelBar.Reset(_progressValues[currentStep]);

            var levelNumber = winCount - currentStep;
            _levelLabels.ForEach(it => it.SetText((++levelNumber).ToString()));
        }
    }
}

using System;
using System.Collections;
using Feofun.UI.Components.Button;
using Survivors.Advertisment.Service;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Components
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonWithAds : ActionButton
    {
        [SerializeField] private float _checkAdsPeriod = 0.2f;
        
        private Coroutine _updateCoroutine;
        
        [Inject] private AdsManager _adsManager;

        public bool OverrideInteractable { get; set; }
        public bool Interactable { get; set; }

        private void OnEnable()
        {
            _updateCoroutine = StartCoroutine(UpdateState());
        }

        private IEnumerator UpdateState()
        {
            while (true)
            {
                Button.interactable = OverrideInteractable ? Interactable : _adsManager.IsRewardAdsReady();
                Debug.Log($"update state. interactable = {Button.interactable} ad ready = {_adsManager.IsRewardAdsReady()}");
                yield return new WaitForSecondsRealtime(_checkAdsPeriod);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(_updateCoroutine);
            _updateCoroutine = null;
        }
    }
}

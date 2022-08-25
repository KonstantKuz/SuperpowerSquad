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
        private bool _overrideInteractable;
        private bool _interactable;
        
        [Inject] private AdsManager _adsManager;

        public void SetOverride(bool interactable)
        {
            _overrideInteractable = true;
            _interactable = interactable;
        }

        public void DeleteOverride()
        {
            _overrideInteractable = false;
            _interactable = _adsManager.IsRewardAdsReady();
        }

        private void OnEnable()
        {
            _updateCoroutine = StartCoroutine(UpdateState());
        }

        private IEnumerator UpdateState()
        {
            while (true)
            {
                Button.interactable = _overrideInteractable ? _interactable : _adsManager.IsRewardAdsReady();
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

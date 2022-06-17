using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

namespace Survivors.Analytics.Wrapper
{
    public class FacebookAnalyticsWrapper : IAnalyticsImpl
    {
        private bool _isInitialized;

        public void Init()
        {
            Debug.Log("Starting initializing Facebook SDK");
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, OnAppVisibilityChange);
            } else {
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized) {
                FB.Mobile.SetAdvertiserTrackingEnabled(true);
                FB.ActivateApp();
                _isInitialized = true;
                Debug.Log("Facebook SDK is Initialized");
            } else {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnAppVisibilityChange(bool isVisible)
        {
            Time.timeScale = !isVisible ? 0 : 1;
        }

        private void LogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
        {
            if (!_isInitialized)
            {
                //TODO: store events while fb sdk not initialized and send them after initialization
                Debug.LogWarning($"Facebook analytics event {logEvent} is lost, cause facebook sdk is not ready yet");
                return;
            }
            FB.LogAppEvent(logEvent, valueToSum, parameters);
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams)
        {
            LogEvent(eventName, null, eventParams);
        }
    }
}
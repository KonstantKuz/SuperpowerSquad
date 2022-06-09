using System.Collections.Generic;
using LegionMaster.Analytics;
using UnityEngine;

namespace Survivors.Analytics.Wrapper
{
    public class AppMetricaAnalyticsWrapper : IAnalyticsImpl
    {
        private bool _isInitialized;

        public void Init()
        {
            Debug.Log("Starting initializing AppMetrica");
            AppMetrica.Instance.OnActivation += OnActivation;
        }
        
        private void OnActivation(YandexAppMetricaConfig config)
        {
            _isInitialized = true;
            Debug.Log("AppMetrica is Initialized");
        }
        
        public void ReportTest()
        {
            ReportEvent("Test", new Dictionary<string, object>());
        }
        
        private void ReportEvent(string message, Dictionary<string, object> parameters)
        {
            if (!_isInitialized)
            {
                //TODO: store events while appmetrica sdk not initialized and send them after initialization
                ShowLostEventWarning(message);
                return;
            }

            AppMetrica.Instance.ReportEvent(message, parameters);
        }

        private static void ShowLostEventWarning(string message)
        {
            Debug.LogWarning($"AppMetrica analytics event {message} is lost, cause appmetrica sdk is not ready yet");
        }

        private void ReportEvent(string message) {
            if (!_isInitialized)
            {
                //TODO: store events while appmetrica sdk not initialized and send them after initialization
                ShowLostEventWarning(message);
                return;
            }

            AppMetrica.Instance.ReportEvent(message);
        }
    }
}
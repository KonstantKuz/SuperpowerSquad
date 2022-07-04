using System;
using System.Collections.Generic;

namespace Survivors.Analytics.Wrapper
{
    public class AppMetricaAnalyticsWrapper : IAnalyticsImpl
    {
        public void Init()
        {
        }
        
        public void ReportTest()
        {
            ReportEvent("Test", new Dictionary<string, object>());
        }
        
        private void ReportEvent(string message, Dictionary<string, object> parameters)
        {
            AppMetrica.Instance.ReportEvent(message, parameters);
        }

        private void ReportEvent(string message) {
            AppMetrica.Instance.ReportEvent(message);
        }

        public void ReportEventWithParams(string eventName, 
            Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            ReportEvent(eventName, eventParams);
            UpdateProfileParams(eventName, eventParams, eventParamProvider);
        }

        private static void UpdateProfileParams(string eventName, 
            Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            var additionalParams = eventParamProvider.GetParams(new []
            {
                EventParams.WINS,
                EventParams.DEFEATS,
                EventParams.PASS_NUMBER
            });
            var profile = new YandexAppMetricaUserProfile();
            var updates = new List<YandexAppMetricaUserProfileUpdate>
            {
                BuildStringAttribute("last_event", BuildLastEventName(eventName, eventParams)), 
                BuildFloatAttribute("kills", eventParams[EventParams.TOTAL_KILLS]), 
                BuildFloatAttribute("level_id", eventParams[EventParams.LEVEL_ID]),
                BuildFloatAttribute("wins", additionalParams[EventParams.WINS]),
                BuildFloatAttribute("defeats", additionalParams[EventParams.DEFEATS]),
                BuildFloatAttribute("levels", eventParams[EventParams.LEVEL_NUMBER]),
                BuildFloatAttribute("level_retry", additionalParams[EventParams.PASS_NUMBER])
            };
            profile.ApplyFromArray(updates);
            AppMetrica.Instance.ReportUserProfile(profile);
        }

        private static YandexAppMetricaUserProfileUpdate BuildFloatAttribute(string name, object value)
        {
            return new YandexAppMetricaNumberAttribute(name).WithValue(Convert.ToDouble(value));
        }
        
        private static YandexAppMetricaUserProfileUpdate BuildStringAttribute(string name, string value)
        {
            return new YandexAppMetricaStringAttribute(name).WithValue(value);
        }

        private static string BuildLastEventName(string eventName, Dictionary<string,object> eventParams)
        {
            return eventName switch
            {
                Events.LEVEL_START => $"level_start_{eventParams[EventParams.LEVEL_ID]}",
                Events.LEVEL_FINISHED => $"level_finished_{eventParams[EventParams.LEVEL_ID]}_{eventParams[EventParams.LEVEL_RESULT]}",
                Events.LEVEL_UP => $"squad_level_{eventParams[EventParams.LEVEL_ID]}_{eventParams[EventParams.SQUAD_LEVEL]}",
                _ => throw new ArgumentOutOfRangeException(nameof(eventName), eventName, null)
            };
        }
    }
}
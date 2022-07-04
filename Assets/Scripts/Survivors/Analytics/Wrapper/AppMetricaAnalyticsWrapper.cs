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
                EventParams.DEFEATS
            });
            var profile = new YandexAppMetricaUserProfile();
            var updates = new List<YandexAppMetricaUserProfileUpdate>
            {
                new YandexAppMetricaStringAttribute("last_event").WithValue(
                    BuildLastEventName(eventName, eventParams)), 
                new YandexAppMetricaNumberAttribute("kills").WithValue(Convert.ToDouble(eventParams[EventParams.TOTAL_KILLS])), 
                new YandexAppMetricaNumberAttribute("level_id").WithValue(Convert.ToDouble(eventParams[EventParams.LEVEL_ID])),
                new YandexAppMetricaNumberAttribute("wins").WithValue(Convert.ToDouble(additionalParams[EventParams.WINS])),
                new YandexAppMetricaNumberAttribute("wins").WithValue(Convert.ToDouble(additionalParams[EventParams.DEFEATS]))
            };
            profile.ApplyFromArray(updates);
            AppMetrica.Instance.ReportUserProfile(profile);
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
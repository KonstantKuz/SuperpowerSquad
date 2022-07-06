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

        private static void UpdateProfileParams(string eventName, Dictionary<string, object> eventParams, IEventParamProvider eventParamProvider)
        {
            var additionalParams = RequestAdditionalParams(eventName, eventParams, eventParamProvider);
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
            TryAddReviveCount(eventParams, updates);
            profile.ApplyFromArray(updates);
            AppMetrica.Instance.ReportUserProfile(profile);
        }

        private static void TryAddReviveCount(IReadOnlyDictionary<string, object> eventParams, ICollection<YandexAppMetricaUserProfileUpdate> updates)
        {
            if (eventParams.ContainsKey(EventParams.REVIVE_COUNT))
            {
                updates.Add(BuildFloatAttribute($"revives_{Convert.ToInt32(eventParams[EventParams.LEVEL_ID])}",
                    eventParams[EventParams.REVIVE_COUNT]));
            }
        }

        private static Dictionary<string, object> RequestAdditionalParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            var additionalParams = eventParamProvider.GetParams(new[]
            {
                EventParams.WINS,
                EventParams.DEFEATS,
                EventParams.PASS_NUMBER
            });
            if (eventName == Events.LEVEL_FINISHED)
            {
                AddLevelResultToWinDefeatCount(eventParams, additionalParams);
            }

            return additionalParams;
        }

        private static void AddLevelResultToWinDefeatCount(IReadOnlyDictionary<string, object> eventParams, IDictionary<string, object> additionalParams)
        {
            additionalParams[EventParams.WINS] = Convert.ToInt32(additionalParams[EventParams.WINS]) +
                                                 ((string)eventParams[EventParams.LEVEL_RESULT] == LevelResult.WIN ? 1 : 0);
            
            additionalParams[EventParams.DEFEATS] = Convert.ToInt32(additionalParams[EventParams.DEFEATS]) +
                                                    ((string)eventParams[EventParams.LEVEL_RESULT] == LevelResult.LOSE
                                                        ? 1
                                                        : 0);
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
                Events.META_UPGRADE_LEVEL_UP => "upgrade_buy",
                Events.REVIVE => $"revive_{eventParams[EventParams.LEVEL_ID]}_{eventParams[EventParams.REVIVE_COUNT]}",
                _ => throw new ArgumentOutOfRangeException(nameof(eventName), eventName, null)
            };
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Logger.Extension;

namespace Survivors.Analytics.Wrapper
{
    public class LoggingAnalyticsWrapper : IAnalyticsImpl
    {
        private bool _enabled;
        
        public void Init()
        {
            _enabled = true;
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            if (!_enabled) return;
            this.Logger().Info($"Event: {eventName}, Params: {DictionaryToString(eventParams)}");
        }

        private static string DictionaryToString(Dictionary<string, object> dict)
        {
            if (dict == null) return "<null>";
            var strings = dict.Select(it => $"{it.Key},{it.Value}");
            return string.Join("\n", strings);
        }
    }
}
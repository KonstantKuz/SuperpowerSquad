﻿using System.Collections.Generic;
using System.Linq;
using Logger.Assets.Scripts;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Analytics.Wrapper
{
    public class LoggingAnalyticsWrapper : IAnalyticsImpl
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger<LoggingAnalyticsWrapper>();
        
        private bool _enabled;
        
        public void Init()
        {
#if UNITY_EDITOR
            _enabled = true;
#endif
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams)
        {
            if (!_enabled) return;
            _logger.Info($"Event: {eventName}, Params: {DictionaryToString(eventParams)}");
        }

        private static string DictionaryToString(Dictionary<string, object> dict)
        {
            if (dict == null) return "<null>";
            var strings = dict.Select(it => $"{it.Key},{it.Value}");
            return string.Join("\n", strings);
        }
    }
}
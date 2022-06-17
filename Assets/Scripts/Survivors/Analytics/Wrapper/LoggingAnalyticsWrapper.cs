﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Analytics.Wrapper
{
    public class LoggingAnalyticsWrapper: IAnalyticsImpl
    {
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
            Debug.Log($"Event: {eventName}, Params: {DictionaryToString(eventParams)}");
        }

        private static string DictionaryToString(Dictionary<string, object> dict)
        {
            if (dict == null) return "<null>";
            var strings = dict.Select(it => $"{it.Key},{it.Value}");
            return string.Join("\n", strings);
        }
    }
}
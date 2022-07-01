using System.Collections.Generic;
using System.Linq;
using AppsFlyerSDK;
using UnityEngine;

namespace Survivors.Analytics.Wrapper
{
    public class AppsFlyerAnalyticsWrapper: IAnalyticsImpl
    {
        private const string DEV_KEY = "9gdCn4p9McTuPMAjnzTk4Y";
        private const string APP_ID = "1626072143";

        public void Init()
        {
            Debug.Log("Initializing AppsFlyer SDK");
            AppsFlyer.initSDK(DEV_KEY, APP_ID);
            AppsFlyer.startSDK();
        }

        private void ReportEvent(string message, Dictionary<string, string> parameters)
        {
            AppsFlyer.sendEvent(message, parameters);
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams)
        {
            eventParams ??= new Dictionary<string, object>();
            ReportEvent(eventName, eventParams.ToDictionary(it => it.Key, it => it.Value.ToString()));
        }
    }
}
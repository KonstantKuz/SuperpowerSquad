using System.Collections.Generic;
using LegionMaster.Analytics;
using AppsFlyerSDK;
using UnityEngine;

namespace Survivors.Analytics.Wrapper
{
    public class AppsFlyerAnalyticsWrapper: IAnalyticsImpl
    {
        public void Init()
        {
            Debug.Log("Initializing AppsFlyer SDK");
            AppsFlyer.initSDK("9gdCn4p9McTuPMAjnzTk4Y", "1626072143");
            AppsFlyer.startSDK();
        }

        public void ReportTest()
        {
            ReportEvent("Test", new Dictionary<string, string>());    
        }
        
        private void ReportEvent(string message, Dictionary<string, string> parameters)
        {
            AppsFlyer.sendEvent(message, parameters);
        }
    }
}
using System.Collections.Generic;
using Survivors.Player.Model;
using Survivors.Session.Config;

namespace Survivors.Analytics
{
    public interface IAnalyticsImpl
    {
        void Init();
        void ReportTest();
        void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams);
    }
}
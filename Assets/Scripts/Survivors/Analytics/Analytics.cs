using System.Collections.Generic;
using LegionMaster.Analytics;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Survivors.Analytics
{
    public class Analytics
    {
        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            Debug.Log("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            _impls.ForEach(it => it.ReportTest());
        }
    }
}
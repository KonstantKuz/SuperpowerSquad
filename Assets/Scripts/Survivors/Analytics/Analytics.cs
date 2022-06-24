using System.Collections.Generic;
using JetBrains.Annotations;
using Logger.Assets.Scripts;
using Logger.Assets.Scripts.Extension;
using ILogger = Logger.Assets.Scripts.ILogger;
using Zenject;

namespace Survivors.Analytics
{
    [PublicAPI]
    public class Analytics
    {
        public const char SEPARATOR = '_';

        [Inject] 
        private IEventParamProvider _eventParamProvider;
        
        
        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            this.Logger().Info("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            ReportEventToAllImpls(Events.TEST_EVENT, null);
        }

        public void ReportLevelStart()
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_ID,
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_LOOP,
                EventParams.PASS_NUMBER
            });
            
            ReportEventToAllImpls(Events.LEVEL_START, eventParams);
        }

        private void ReportEventToAllImpls(string eventName, Dictionary<string, object> eventParams)
        {
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(eventName, eventParams);
            }
        }

        public void ReportLevelUp(string upgradeBranch)
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_ID,
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_LOOP,
                EventParams.SQUAD_LEVEL,
                $"{EventParams.UPGRADE}{SEPARATOR}{upgradeBranch}",
                EventParams.ENEMY_KILLED,
                EventParams.TIME_SINCE_LEVEL_START
            });
            
            ReportEventToAllImpls(Events.LEVEL_UP, eventParams);
        }

        public void ReportLevelFinished(bool isPlayerWinner)
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_ID,
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_LOOP,
                EventParams.PASS_NUMBER,
                EventParams.SQUAD_LEVEL,
                EventParams.TIME_SINCE_LEVEL_START,
                EventParams.ENEMY_KILLED,
                EventParams.TOTAL_ENEMY_HEALTH,
                EventParams.AVERAGE_ENEMY_LIFETIME,
                EventParams.STAND_RATIO
            });
            
            eventParams[EventParams.LEVEL_RESULT] = isPlayerWinner ? "win" : "lose";
            
            ReportEventToAllImpls(Events.LEVEL_FINISHED, eventParams);
        }
    }
}
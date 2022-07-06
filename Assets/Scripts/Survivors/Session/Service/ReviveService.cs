using System.Linq;
using ModestTree;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session.Service
{
    public class ReviveService
    {
        [Inject] private World _world;     
        [Inject] private UnitService _unitService;
        [Inject] private ConstantsConfig _constantsConfig;  
        [Inject] private SessionService _sessionService;    
        [Inject] private Analytics.Analytics _analytics;        
        
        public void Revive()
        {
            Assert.IsNotNull(_world.Squad, "Should call this method only inside game session");
            _world.Squad.RestoreHealth();
            KillEnemiesAroundSquad();
            _sessionService.AddRevive();
            _analytics.ReportRevive();            
        }
        
        private void KillEnemiesAroundSquad()
        {
            var enemiesNearby = _unitService.GetUnitsInRadius(_world.Squad.Position, UnitType.ENEMY,
                _constantsConfig.ReviveEnemyRemoveRadius).ToList();
            enemiesNearby.ForEach(it => it.Kill(DeathCause.Removed));
        }        
    }
}
using Survivors.Session.Config;
using Survivors.Units;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly LevelMissionConfig _levelMissionConfig;
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        
        public bool IsMaxKills => Kills >= _levelMissionConfig.KillCount;
        
        private Session(LevelMissionConfig levelMissionConfig)
        {
            _levelMissionConfig = levelMissionConfig;
        }
        public static Session Build(LevelMissionConfig levelMissionConfig) => new Session(levelMissionConfig);
        
        public void SetResultByUnitType(UnitType unitType)
        {
            Result = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }
        public void AddKill() => Kills++;
        
    }
}
using Survivors.Session.Config;
using Survivors.Units;
using UnityEngine;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly LevelMissionConfig _levelMissionConfig;
        private readonly float _startTime;        
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        
        public bool IsMaxKills => Kills >= _levelMissionConfig.KillCount;

        public bool Completed => Result.HasValue;
        
        private Session(LevelMissionConfig levelMissionConfig)
        {
            _levelMissionConfig = levelMissionConfig;
            _startTime = Time.time;
        }
        public static Session Build(LevelMissionConfig levelMissionConfig) => new Session(levelMissionConfig);
        
        public void SetResultByUnitType(UnitType unitType)
        {
            Result = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }
        public void AddKill() => Kills++;
        
        public float SessionTime => Time.time - _startTime;        
    }
}
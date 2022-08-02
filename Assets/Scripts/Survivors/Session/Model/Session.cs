using System.Linq;
using Survivors.Enemy.Spawn.Config;
using Survivors.Units;
using UnityEngine;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly LevelWavesConfig _levelWavesConfig;
        private readonly float _startTime;        
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        public int Revives { get; private set; }
        
        public bool IsMaxKills => Kills >= _levelWavesConfig.Waves.Sum(it => it.Count);

        public bool Completed => Result.HasValue;

        public int Level => _levelWavesConfig.Level;
        
        private Session(LevelWavesConfig levelWavesConfig)
        {
            _levelWavesConfig = levelWavesConfig;
            _startTime = Time.time;
        }
        public static Session Build(LevelWavesConfig levelWavesConfig) => new Session(levelWavesConfig);
        
        public void SetResultByUnitType(UnitType unitType)
        {
            Result = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }
        public void AddKill() => Kills++;

        public void AddRevive() => Revives++;
        
        public float SessionTime => Time.time - _startTime;        
    }
}
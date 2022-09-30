using System;
using Survivors.Scope.Timer;
using Survivors.Session.Config;
using Survivors.Units;
using UniRx;
using UnityEngine;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly LevelMissionConfig _levelMissionConfig;
        private readonly IScopeTime _spawnTime;
        private float _startTime;
        private IReadOnlyReactiveProperty<float> _playTime; 

        
        private Session(LevelMissionConfig levelMissionConfig, IScopeTime spawnTime)
        {
            _levelMissionConfig = levelMissionConfig;
            _spawnTime = spawnTime;
        }
        public static Session Build(LevelMissionConfig levelMissionConfig, IScopeTime spawnTime) => new Session(levelMissionConfig, spawnTime);
        
        private bool IsMaxKills => Kills >= _levelMissionConfig.KillCount;
        private bool IsMaxSpawnTime => _spawnTime.Time >= _levelMissionConfig.Time;
        
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        public int Revives { get; private set; }
        
        public bool Completed => Result.HasValue;

        public LevelMissionConfig LevelMissionConfig => _levelMissionConfig;
        
        public float SessionTime => Time.time - _startTime;
        public IReadOnlyReactiveProperty<float> PlayTime => _playTime;
        public IScopeTime SpawnTime => _spawnTime;

        public void Start()
        {
            _startTime = Time.time;
            _playTime = Observable.Interval(TimeSpan.FromSeconds(1)).Select(it => (float) it + 1).ToReactiveProperty();
        }
        
        public void AddKill() => Kills++;
        public void AddRevive() => Revives++;

        public void SetResultByUnitType(UnitType unitType)
        {
            Result = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }

        public bool IsMissionGoalReached()
        {
            switch (_levelMissionConfig.MissionType)
            {
                case LevelMissionType.KillCount:
                    return IsMaxKills;
                case LevelMissionType.Time:
                    return IsMaxSpawnTime;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_levelMissionConfig.MissionType}");
            }
        }
    }
}
using System;
using Feofun.Components;
using Logger.Extension;
using Survivors.Session.Config;
using Survivors.Units;
using UniRx;
using UnityEngine;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly LevelMissionConfig _levelMissionConfig;
        private readonly float _startTime;
        private readonly IReadOnlyReactiveProperty<float> _playTime;
        
        private Session(LevelMissionConfig levelMissionConfig)
        {
            _levelMissionConfig = levelMissionConfig;
            _startTime = Time.time;
            _playTime = Observable.Interval(TimeSpan.FromSeconds(1)).Select(it => (float) it).ToReactiveProperty();
        }
        
        private bool IsMaxKills => Kills >= _levelMissionConfig.KillCount;
        private bool IsMaxTime => _playTime.Value >= _levelMissionConfig.Time;
        
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        public int Revives { get; private set; }
        
        public bool Completed => Result.HasValue;

        public LevelMissionConfig LevelMissionConfig => _levelMissionConfig;
        
        public float SessionTime => Time.time - _startTime;
        public IReadOnlyReactiveProperty<float> PlayTime => _playTime;
        
        public static Session Build(LevelMissionConfig levelMissionConfig) => new Session(levelMissionConfig);

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
                    return IsMaxTime;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_levelMissionConfig.MissionType}");
            }
        }
    }
}
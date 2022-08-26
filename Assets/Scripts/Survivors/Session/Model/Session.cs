using System;
using Feofun.Components;
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
        private readonly PlayTimer _playTimer;
        
        private Session(LevelMissionConfig levelMissionConfig, ICoroutineRunner coroutineRunner)
        {
            _levelMissionConfig = levelMissionConfig;
            _startTime = Time.time;
            _playTimer = new PlayTimer(coroutineRunner);
        }
        
        private bool IsMaxKills => Kills >= _levelMissionConfig.KillCount;
        private bool IsMaxTime => _playTimer.PlayTime.Value >= _levelMissionConfig.Time;
        
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        public int Revives { get; private set; }
        
        public bool Completed => Result.HasValue;

        public LevelMissionConfig LevelMissionConfig => _levelMissionConfig;
        
        public float SessionTime => Time.time - _startTime;
        public IReadOnlyReactiveProperty<float> PlayTime => _playTimer.PlayTime;
        
        public static Session Build(LevelMissionConfig levelMissionConfig, ICoroutineRunner coroutineRunner) => 
            new Session(levelMissionConfig, coroutineRunner);

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

        public void StopTimer()
        {
            _playTimer.Stop();
        }
    }
}
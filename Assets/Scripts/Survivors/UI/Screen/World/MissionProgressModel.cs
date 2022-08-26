using System;
using Survivors.Session.Config;
using Survivors.Session.Service;
using UniRx;

namespace Survivors.UI.Screen.World
{
    public class MissionProgressModel
    {
        private const string CHAPTER_LOCALIZATION_ID = "Chapter";
        private const string SECONDS_LOCALIZATION_ID = "Seconds";
        
        private readonly LevelMissionConfig _levelConfig;
        private readonly IReadOnlyReactiveProperty<float> _playTime;
        
        public LevelMissionType MissionType { get; }
        public string LabelId { get; }
        public float LeftSeconds => _levelConfig.Time - _playTime.Value;
        public IReadOnlyReactiveProperty<float> LevelProgress { get; }
        public MissionProgressModel(LevelMissionConfig levelConfig, 
            IReadOnlyReactiveProperty<int> killsCount, 
            IReadOnlyReactiveProperty<float> playTime)
        {
            _levelConfig = levelConfig;
            _playTime = playTime;
            
            MissionType = _levelConfig.MissionType;
            
            LabelId = _levelConfig.MissionType switch
            {
                LevelMissionType.KillCount => CHAPTER_LOCALIZATION_ID,
                LevelMissionType.Time => SECONDS_LOCALIZATION_ID,
                _ => throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_levelConfig.MissionType}")
            };
            
            LevelProgress = _levelConfig.MissionType switch
            {
                LevelMissionType.KillCount => killsCount.Select(count => (float) count / _levelConfig.KillCount).ToReactiveProperty(),
                LevelMissionType.Time => playTime.Select(time => time / _levelConfig.Time).ToReactiveProperty(),
                _ => throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_levelConfig.MissionType}")
            };
        }
    }
}
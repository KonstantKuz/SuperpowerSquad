using System;
using Feofun.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Session.Config;
using Survivors.Units.Enemy.Config;
using UniRx;

namespace Survivors.UI.Screen.World.Mission
{
    public class MissionProgressModel
    {
        private const string CHAPTER_LOCALIZATION_ID = "Chapter";
        private const string SECONDS_LOCALIZATION_ID = "Seconds";
        
        private readonly LevelMissionConfig _levelConfig;

        public LevelMissionType MissionType { get; }
        public string LabelId { get; private set; }
        public IReadOnlyReactiveProperty<string> LabelContent { get; private set; }
        public IReadOnlyReactiveProperty<float> LevelProgress { get; private set; }
        public MissionEventModel MissionEventModel { get; private set; }

        public MissionProgressModel(LevelMissionConfig levelConfig, 
            IReadOnlyReactiveProperty<int> killsCount, 
            IReadOnlyReactiveProperty<float> spawnTime,
            EnemyWavesConfig wavesConfig, 
            ConfigCollection<string, EnemyUnitConfig> enemyUnitConfig)
        {
            _levelConfig = levelConfig;
            
            MissionType = _levelConfig.MissionType;
            switch (MissionType)
            {
                case LevelMissionType.KillCount:
                    InitForKillCountMission(killsCount);
                    break;
                case LevelMissionType.Time:
                    InitForTimeMission(spawnTime, wavesConfig, enemyUnitConfig);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected mission type := {MissionType}");
            }
        }

        private void InitForKillCountMission(IReadOnlyReactiveProperty<int> killsCount)
        {
            LabelId = CHAPTER_LOCALIZATION_ID;
            LabelContent = new StringReactiveProperty(_levelConfig.Level.ToString());
            LevelProgress = killsCount.Select(count => (float) count / _levelConfig.KillCount).ToReactiveProperty();
        }

        private void InitForTimeMission(IReadOnlyReactiveProperty<float> spawnTime,
            EnemyWavesConfig wavesConfig, 
            ConfigCollection<string, EnemyUnitConfig> enemyUnitConfig)
        {
            LabelId = SECONDS_LOCALIZATION_ID;
            LabelContent = spawnTime.Select(time =>  Convert.ToInt32(_levelConfig.Time - time).ToString()).ToReactiveProperty();
            LevelProgress = spawnTime.Select(time => time / _levelConfig.Time).ToReactiveProperty();
            MissionEventModel = new MissionEventModel(wavesConfig, enemyUnitConfig, _levelConfig.Time);
        }
    }
}
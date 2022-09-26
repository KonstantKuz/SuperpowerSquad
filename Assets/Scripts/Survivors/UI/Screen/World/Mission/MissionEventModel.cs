using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Units.Enemy.Config;
using Survivors.Util;
using UnityEngine;

namespace Survivors.UI.Screen.World.Mission
{
    public class MissionEventModel
    {
        public struct MissionEvent
        {
            public float Progress;
            public Sprite Icon;
        }

        public readonly List<MissionEvent> Events;

        public MissionEventModel(EnemyWavesConfig wavesConfig, 
            ConfigCollection<string, EnemyUnitConfig> enemyUnitConfig,
            float missionTime)
        {
            Events = wavesConfig.EnemySpawns
                .Where( it => enemyUnitConfig.Get(it.EnemyId).IsBoss)
                .Select(it => new MissionEvent
                {
                    Progress = it.SpawnTime / missionTime,
                    Icon = Resources.Load<Sprite>(IconPath.GetMissionEvent(it.EnemyId))
                })
                .ToList();
        }
    }
}
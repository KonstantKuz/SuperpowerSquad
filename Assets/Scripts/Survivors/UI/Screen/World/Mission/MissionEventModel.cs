using System.Collections.Generic;
using System.Linq;
using Survivors.Enemy.Spawn;
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

        public MissionEventModel(EnemySpawnService enemySpawnService, 
                                 float missionTime)
        {
            Events = enemySpawnService.GetEnemyWavesConfig(true)
                                      .Select(it => new MissionEvent
                                      {
                                              Progress = it.SpawnTime / missionTime,
                                              Icon = Resources.Load<Sprite>(IconPath.GetMissionEvent(it.EnemyId))
                                      }).ToList();
        }
    }
}
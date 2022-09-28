using System.Collections.Generic;
using System.Linq;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Service;
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

        public MissionEventModel(EnemyWaves enemyWaves, 
                                 float missionTime)
        {
            Events = enemyWaves.GetWavesConfigs(true)
                               .Select(it => new MissionEvent
                               {
                                       Progress = it.SpawnTime / missionTime,
                                       Icon = Resources.Load<Sprite>(IconPath.GetMissionEvent(it.EnemyId))
                               }).ToList();
        }
    }
}
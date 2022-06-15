using System;
using Survivors.Squad.Service;
using UniRx;

namespace Survivors.UI.Screen.World.SquadProgress
{
    public class SquadProgressModel
    {
        public readonly IObservable<float> LevelProgress;
        public readonly IObservable<int> Level;

        public SquadProgressModel(SquadProgressService squadProgressService)
        {
            LevelProgress = squadProgressService.Exp.Select(it => {
                                                    if (squadProgressService.CurrentLevelConfig == null) {
                                                        return 0;
                                                    }
                                                    return (float) it / squadProgressService.CurrentLevelConfig.ExpToNextLevel;
                                                })
                                                .AsObservable();
            Level = squadProgressService.Level;
        }
    }
} 
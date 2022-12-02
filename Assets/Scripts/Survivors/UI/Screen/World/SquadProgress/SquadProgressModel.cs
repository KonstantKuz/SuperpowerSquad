using System;
using Survivors.Squad.Data;
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
            LevelProgress = squadProgressService.GetAsObservable(SquadProgressType.Exp)
                .Select(it => (float) it / squadProgressService.CurrentLevelConfig.ExpToNextLevel)
                .AsObservable();
            Level = squadProgressService.GetAsObservable(SquadProgressType.Level);
        }
    }
} 
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Survivors.Player.Progress.Model
{
    public class PlayerProgress
    {
        [JsonProperty]
        private readonly Dictionary<int, int> _passCount = new Dictionary<int, int>();

        public int GameCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount => GameCount - WinCount;
        public int LevelNumber => WinCount;
        public int Kills { get; set; }
        public int Revives { get; set; }

        public int GetPassCount(int levelId) => _passCount.ContainsKey(levelId) ? _passCount[levelId] : 0;

        public void IncreasePassCount(int levelId)
        {
            _passCount[levelId] = GetPassCount(levelId) + 1;
        }

        public static PlayerProgress Create() => new PlayerProgress();
 
    }
}
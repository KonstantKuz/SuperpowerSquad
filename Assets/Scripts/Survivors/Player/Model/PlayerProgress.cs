
namespace Survivors.Player.Model
{
    public class PlayerProgress
    {
        public int GameCount { get; set; }
        public int WinCount { get; set; }
        public int LevelNumber => WinCount;

        public static PlayerProgress Create() => new PlayerProgress();
 
    }
}
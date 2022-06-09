
namespace Survivors.Player.Progress
{
    public class PlayerProgress
    {
        public int SessionCount { get; set; }
        public int WinCount { get; set; }
        
        public static PlayerProgress Create() => new PlayerProgress();
 
    }
}
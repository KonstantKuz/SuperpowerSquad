using Survivors.Units;

namespace Survivors.Session.Model
{
    public class Session
    {
        public readonly int CurrentLevel;
        public BattleResult? Winner { get; private set; }
        protected Session(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
        public static Session Build(int currentLevel) => new Session(currentLevel);
        
        public void SetWinnerByUnitType(UnitType unitType)
        {
            Winner = unitType == UnitType.PLAYER ? BattleResult.Win : BattleResult.Lose;
        }
    }
}
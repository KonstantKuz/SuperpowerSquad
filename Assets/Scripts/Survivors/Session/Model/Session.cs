using Survivors.Enemy.Config;
using Survivors.Units;

namespace Survivors.Session.Model
{
    public class Session
    {
        private readonly EnemyLevelConfig EnemyLevelConfig;
        public int Kills { get; private set; }
        public SessionResult? Winner { get; private set; }
        
        public bool IsMaxKills => Kills >= EnemyLevelConfig.KillCount;
        
        private Session(EnemyLevelConfig enemyLevelConfig)
        {
            EnemyLevelConfig = enemyLevelConfig;
        }
        public static Session Build(EnemyLevelConfig enemyLevelConfig) => new Session(enemyLevelConfig);
        
        public void SetWinnerByUnitType(UnitType unitType)
        {
            Winner = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }
        public void AddKill() => Kills++;
        
    }
}
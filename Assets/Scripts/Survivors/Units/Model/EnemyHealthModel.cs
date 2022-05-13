namespace Survivors.Units.Model
{
    public class EnemyHealthModel : IUnitHealthModel
    {
        public int MaxHealth { get; set; }
        public int StartingHealth { get; set; }
    }
}
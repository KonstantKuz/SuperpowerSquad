using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; set; }
        public IUnitHealthModel HealthModel { get; set; }

        public static EnemyUnitModel Create(string id, 
            IUnitHealthModel healthModel)
        {
            return new EnemyUnitModel
            {
                Id = id,
                HealthModel = healthModel,
            };
        }
    }
}
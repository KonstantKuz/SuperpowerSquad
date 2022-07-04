using Survivors.Units.Player.Model;
using Survivors.Units.Player.Model.Session;

namespace Survivors.Units.Weapon.Projectiles.Params
{
    public class PlayerProjectileParams : IProjectileParams
    {
        private PlayerAttackModel _attackModel;
        public float AdditionalAttackDistance { get; set; }
        
        public float Speed => _attackModel.ProjectileSpeed;
        public float DamageRadius => _attackModel.DamageRadius;
        public float AttackDistance => AdditionalAttackDistance + _attackModel.AttackDistance;
        public int Count => _attackModel.ShotCount.Value;

        public PlayerProjectileParams(PlayerAttackModel attackModel)
        {
            _attackModel = attackModel;
            AdditionalAttackDistance = 0;
        }
    }
}
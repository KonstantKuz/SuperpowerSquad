using Survivors.Units.Player.Model;
using Survivors.Units.Player.Model.Session;

namespace Survivors.Units.Weapon.Projectiles.Params
{
    public class PlayerProjectileParams : IProjectileParams
    {
        private PlayerAttackSessionModel _attackSessionModel;
        public float AdditionalAttackDistance { get; set; }
        
        public float Speed => _attackSessionModel.ProjectileSpeed;
        public float DamageRadius => _attackSessionModel.DamageRadius;
        public float AttackDistance => AdditionalAttackDistance + _attackSessionModel.AttackDistance;
        public int Count => _attackSessionModel.ShotCount.Value;

        public PlayerProjectileParams(PlayerAttackSessionModel attackSessionModel)
        {
            _attackSessionModel = attackSessionModel;
            AdditionalAttackDistance = 0;
        }
    }
}
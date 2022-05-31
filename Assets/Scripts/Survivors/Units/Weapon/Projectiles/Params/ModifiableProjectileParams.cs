using Survivors.Units.Player.Model;

namespace Survivors.Units.Weapon.Projectiles.Params
{
    public class ModifiableProjectileParams : IProjectileParams
    {
        private PlayerAttackModel _attackModel;
        public float AdditionalAttackDistance { get; set; }
        
        public float Speed => _attackModel.ProjectileSpeed;
        public float DamageRadius => _attackModel.DamageRadius;
        public float AttackDistance => AdditionalAttackDistance + _attackModel.AttackDistance;
        public int Count => _attackModel.ShotCount.Value;

        public ModifiableProjectileParams(PlayerAttackModel attackModel)
        {
            _attackModel = attackModel;
            AdditionalAttackDistance = 0;
        }
    }
}
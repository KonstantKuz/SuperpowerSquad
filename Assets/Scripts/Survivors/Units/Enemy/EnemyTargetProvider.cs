using JetBrains.Annotations;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;

namespace Survivors.Units.Enemy
{
    public class EnemyTargetProvider
    {
        private readonly ITargetSearcher _targetSearcher;
        private ITarget _target;
        
        public EnemyTargetProvider(ITargetSearcher targetSearcher)
        {
            _targetSearcher = targetSearcher;
        }
        
        [CanBeNull] 
        public ITarget CurrentTarget
        {
            get
            {
                if (_target == null)
                {
                    CurrentTarget = _targetSearcher.Find();
                }

                return _target;
            }
            private set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.OnTargetInvalid -= ClearTarget;
                }
                _target = value;
                if (_target != null)
                {
                    _target.OnTargetInvalid += ClearTarget;
                }
            }
        }

        private void ClearTarget()
        {
            CurrentTarget = null;
        }
    }
}
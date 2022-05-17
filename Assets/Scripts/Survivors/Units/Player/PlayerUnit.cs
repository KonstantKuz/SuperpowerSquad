using Survivors.Location.Model;
using Survivors.Units.Player.Model;

namespace Survivors.Units.Player
{
    public class PlayerUnit : WorldObject
    {
        private IUpdatableUnitComponent[] _updatables;
        public PlayerUnitModel Model { get; private set; }
        
        public void Init(PlayerUnitModel model)
        {
            Model = model;
            _updatables = GetComponentsInChildren<IUpdatableUnitComponent>(); 
            foreach (var component in GetComponentsInChildren<IUnitInitializable>()) {
                component.Init(this);
            }
        }
        private void Update()
        {
            UpdateComponents();
        }
        private void UpdateComponents()
        {
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }
    }
}
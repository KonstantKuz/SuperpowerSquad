using Survivors.Location.Model;
using Survivors.Units.Model;

namespace Survivors.Units
{
    public class Unit : WorldObject, IUnit
    {
        private IUpdatableUnitComponent[] _updatables;
        public IUnitModel Model { get; private set; }

        public void Init(IUnitModel model)
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
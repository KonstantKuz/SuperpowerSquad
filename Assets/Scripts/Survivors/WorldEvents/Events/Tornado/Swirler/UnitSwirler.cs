using Feofun.Components;
using Survivors.Units;

namespace Survivors.WorldEvents.Events.Tornado.Swirler
{
    public class UnitSwirler : Swirler, IInitializable<IUnit>
    {
        private IUnit _unit;


        private float time;
        
        public void Init(IUnit owner)
        {
            _unit = owner;
        }

        protected override void Capture()
        {
            _unit.IsActive = false;
        }

        protected override void Release()
        {
            _unit.IsActive = true;
        }
    }
}
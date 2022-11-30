using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class CircleFormation: ISquadFormation
    {
        private readonly float _initialRadius;
        private readonly float _increaseStep;
        
        public CircleFormation(float initialRadius, float increaseStep)
        {
            _initialRadius = initialRadius;
            _increaseStep = increaseStep;
        }
        
        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            var angle = 360 * unitIdx / unitsCount;
            var radius = _initialRadius + _increaseStep * unitsCount * unitRadius / Mathf.PI / 2;
            return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * radius;
        }
    }
}
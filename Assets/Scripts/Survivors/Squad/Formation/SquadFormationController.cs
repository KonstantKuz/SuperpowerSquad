using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Feofun.Extension;
using UniRx;
using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class SquadFormationController : MonoBehaviour, IInitializable<Squad>
    {
        [SerializeField] private float _unitSize;   
        
        private Squad _squad;
        private ISquadFormation _formation;
        private CompositeDisposable _disposable;

        private void Awake()
        {
            _squad = gameObject.RequireComponent<Squad>();
        }

        public void Init(Squad owner)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _formation = new CircleFormation(_squad.Model.InitialRadius, _squad.Model.RadiusIncreaseStep);
            _squad.UnitsCount.Subscribe(UpdateFormation).AddTo(_disposable);
        }

        private void UpdateFormation(int unitsCount)
        {
            var positions = GetUnitOffsets(unitsCount - 1).ToArray();
            for (int i = 0; i < positions.Length; i++)
            {
                _squad.Units[i + 1].transform.localPosition = positions[i];
            }
        }
        
        public float CalculateSquadRadius()
        {
            var radius = _unitSize;
            var positions = GetUnitOffsets(_squad.Units.Count - 1).OrderBy(it => it.magnitude).ToList();
            if (positions.Any())
            {
                var furtherPosition = _squad.Position + positions.Last();
                radius = Vector3.Distance(furtherPosition, _squad.Position) + _unitSize;
            }

            return radius;
        }
        
        private IEnumerable<Vector3> GetUnitOffsets(int unitsCount)
        {
            for (int i = 0; i < unitsCount; i++)
            {
                yield return _formation.GetUnitOffset(i, _unitSize, unitsCount);
            }
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Survivors.App
{
    public class UpdateManager : MonoBehaviour
    {
        [SerializeField] private bool _lockFrameRate;
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private int _reservedObjectsCount = 2000;
    
        private List<Action> _customUpdates;
        private List<Action> _customFixedUpdates;
        private List<Action> _customLateUpdates;     
        
        private List<Action> _prepareUpdates;
        private List<Action> _prepareFixedUpdates;
        private List<Action> _prepareLateUpdates;
    
        private void Awake()
        {
            _customUpdates = new List<Action>(_reservedObjectsCount);
            _customFixedUpdates = new List<Action>(_reservedObjectsCount);
            _customLateUpdates = new List<Action>(_reservedObjectsCount);

            _prepareUpdates = new List<Action>();
            _prepareFixedUpdates = new List<Action>();
            _prepareLateUpdates = new List<Action>();
            
            if (_lockFrameRate)
            {
                Application.targetFrameRate = _targetFrameRate;
            }
        }

        public void StartUpdate(Action obj)
        {
            _customUpdates.Add(obj);
        }

        public void StartFixedUpdate(Action obj)
        {
            _customFixedUpdates.Add(obj);
        }

        public void StartLateUpdate(Action obj)
        {
            _customLateUpdates.Add(obj);
        }
    
        public void StopUpdate(Action obj)
        {
            _customUpdates.Remove(obj);
        }

        public void StopFixedUpdate(Action obj)
        {
            _customFixedUpdates.Remove(obj);
        }

        public void StopLateUpdate(Action obj)
        {
            _customLateUpdates.Remove(obj);
        }
    
        private void Update()
        {
            InvokeAll(_customUpdates);
        }

        private void FixedUpdate()
        {
            InvokeAll(_customFixedUpdates);
        }

        private void LateUpdate()
        {
            InvokeAll(_customLateUpdates);
        }

        private void InvokeAll(List<Action> actions)
        {
            foreach (var action in actions) {
                action.Invoke();
            }
        }
    }
}

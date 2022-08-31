using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T>
    {
        private readonly HashSet<T> _allItems;
        private readonly Stack<T> _inactiveStack;
        private readonly Func<T> _createFunc;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly ObjectPoolParams _poolParams;
        
        private bool _initialCapacityShortageDetected;

        public int CountAll => _allItems.Count;
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _inactiveStack.Count;

        public ObjectPool(Func<T> createFunc,
                          Action<T> onGet = null,
                          Action<T> onRelease = null,
                          Action<T> onDestroy = null,
                          [CanBeNull] ObjectPoolParams poolParams = null)
        {
            _poolParams = poolParams ?? ObjectPoolParams.Default;

            if (createFunc == null) {
                throw new ArgumentNullException(nameof(createFunc));
            }
            if (_poolParams.MaxCapacity <= 0) {
                throw new ArgumentException("Max capacity must be greater than 0", nameof(_poolParams.MaxCapacity));
            }
            if (_poolParams.SizeIncrementStep <= 0) {
                throw new ArgumentException("Size increment step must be greater than 0", nameof(_poolParams.SizeIncrementStep));
            }
            if (_poolParams.InitialCapacity < 0) {
                throw new ArgumentException("Initial capacity must be non-negative", nameof(_poolParams.InitialCapacity));
            }

            _inactiveStack = new Stack<T>(_poolParams.InitialCapacity);
            _allItems = new HashSet<T>();
            _createFunc = createFunc;
            _onGet = onGet;
            _onRelease = onRelease;
            _onDestroy = onDestroy;
            Init(_poolParams.InitialCapacity);
        }

        private void Init(int initialCapacity)
        {
            if (initialCapacity <= 0) {
                return;
            }
            Allocate(initialCapacity);
        }
        public T Get()
        {
            DetectInitialCapacityShortage();
            if (_inactiveStack.Count == 0) {
                Allocate(_poolParams.SizeIncrementStep);
            }
            var item = _inactiveStack.Pop();
            _onGet?.Invoke(item);
            return item;
        }


        public void Release(T element)
        {
            if (_inactiveStack.Count > 0 && _inactiveStack.Contains(element)) {
                throw new InvalidOperationException($"Trying to release an object that has already been released to the pool. Pool type:= {typeof(T)}");
            }
            _onRelease?.Invoke(element);

            if (CountInactive < _poolParams.MaxCapacity) {
                _inactiveStack.Push(element);
            } else {
                Debug.LogWarning($"Object count in the pool has reached the maximum count, max capacity:= {_poolParams.MaxCapacity}, the last element will be destroyed. Pool type:= {typeof(T)}");
                CallOnDestroy(element);
            }
        }

        public void ReleaseAllActive()
        {
            var activeItems = _allItems.Except(_inactiveStack).ToList();
            foreach (var element in activeItems) {
                Release(element);
            }
        }

        private void DetectInitialCapacityShortage()
        {
            if (_initialCapacityShortageDetected) {
                return;
            }
            if (_inactiveStack.Count == 0 && _poolParams.DetectInitialCapacityShortage) {
                Debug.LogWarning($"Shortage of initial capacity, objects will be created, should increase the initial capacity, initial capacity:= {_poolParams.InitialCapacity}. Pool type:= {typeof(T)}");
                _initialCapacityShortageDetected = true;
            }
        }
        private void Allocate(int count)
        {
            for (int i = 0; i < count; i++) {
                var item = Create();
                Release(item);
            }
        }
        private T Create()
        {
            var element = _createFunc();
            _allItems.Add(element);
            return element;
        }
        private void Clear()
        {
            foreach (var element in _inactiveStack) {
                CallOnDestroy(element);
            }
            _inactiveStack.Clear();
            foreach (var element in _allItems) {
                _onDestroy?.Invoke(element);
            }
            _allItems.Clear();
        }

        public void Dispose() => Clear();

        private void CallOnDestroy(T element)
        {
            _allItems.Remove(element);
            _onDestroy?.Invoke(element);
        }
    }
}
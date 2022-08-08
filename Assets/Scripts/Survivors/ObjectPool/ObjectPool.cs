using System;
using System.Collections.Generic;
using System.Linq;

namespace Survivors.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T>, IDisposable
            where T : class
    {
        private readonly HashSet<T> _allItems;
        private readonly Stack<T> _inactiveStack;
        private readonly Func<T> _onCreate;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;

        private readonly int _maxSize;
        private readonly bool _isCollectionCheck;
        private readonly bool _disposeActive;

        private readonly int _initialCapacity;
        private ObjectCreateMode CreateMode { get; set; }

        public int CountAll => _allItems.Count;

        public int CountActive => CountAll - CountInactive;

        public int CountInactive => _inactiveStack.Count;

        public ObjectPool(Func<T> onCreate,
                          Action<T> onGet = null,
                          Action<T> onRelease = null,
                          Action<T> onDestroy = null,
                          bool isCollectionCheck = true,
                          int initialCapacity = 10,
                          int maxSize = 10000,
                          ObjectCreateMode objectCreateMode = ObjectCreateMode.Single,
                          bool disposeActive = true)
        {
            if (onCreate == null) {
                throw new ArgumentNullException(nameof(onCreate));
            }

            if (maxSize <= 0) {
                throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
            }

            _initialCapacity = initialCapacity;
            _inactiveStack = new Stack<T>(initialCapacity);
            _allItems = new HashSet<T>();
            CreateMode = objectCreateMode;
            _onCreate = onCreate;
            _maxSize = maxSize;
            _onGet = onGet;
            _onRelease = onRelease;
            _onDestroy = onDestroy;
            _isCollectionCheck = isCollectionCheck;
            _disposeActive = disposeActive;
        }

        public T Get()
        {
            var element = _inactiveStack.Count == 0 ? Create(CreateMode) : _inactiveStack.Pop();
            _onGet?.Invoke(element);
            return element;
        }

        private T Create(ObjectCreateMode createMode)
        {
            var element = Create();
            if (createMode != ObjectCreateMode.Group) {
                return element;
            }
            for (int i = 0; i < Math.Min(_initialCapacity - 1, _maxSize); i++) {
                var groupElement = Create();
                Release(groupElement);
            }
            return element;
        }
        private T Create()
        {
            var element = _onCreate();
            _allItems.Add(element);
            return element;
        }

        public void ReleaseAllActive()
        {
            var activeElements = _allItems.Except(_inactiveStack);
            foreach (var element in activeElements) {
                Release(element);
            }
        }

        public void Release(T element)
        {
            if (_isCollectionCheck && _inactiveStack.Count > 0 && _inactiveStack.Contains(element)) {
                throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
            }

            _onRelease?.Invoke(element);

            if (CountInactive < _maxSize) {
                _inactiveStack.Push(element);
            } else {
                CallOnDestroy(element);
            }
        }

        public void Clear()
        {
            foreach (var element in _inactiveStack) {
                CallOnDestroy(element);
            }
            _inactiveStack.Clear();
            
            if (_disposeActive) {
                foreach (var element in _allItems) {
                    _onDestroy?.Invoke(element);
                } 
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
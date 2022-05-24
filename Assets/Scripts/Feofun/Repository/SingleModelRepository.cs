using System;
using JetBrains.Annotations;

namespace Feofun.Repository
{
    public class SingleModelRepository<T> : ISingleModelRepository<T>
            where T : class
    {
        [CanBeNull]
        private T _model;

        [CanBeNull]
        public T Get()
        {
            return _model;
        }

        public T Require()
        {
            return Get() ?? throw new NullReferenceException("Object  " + typeof(T) + " not found");
        }

        public bool Exists()
        {
            return Get() != null;
        }

        public void Set(T model)
        {
            _model = model;
        }

        public void Delete()
        {
            _model = null;
        }
    }
}
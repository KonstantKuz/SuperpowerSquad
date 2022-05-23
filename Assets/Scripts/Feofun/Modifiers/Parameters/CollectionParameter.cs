using System.Collections.Generic;

namespace Feofun.Modifiers.Parameters
{
    public class CollectionParameter<T> : IModifiableParameter
    {
        private ICollection<T> _value;
        public ICollection<T> InitialValue { get; }
        public string Name { get; }

        public IReadOnlyCollection<T> Value => new List<T>(_value); //no other existing way in c# to convert ICollection to IReadOnlyCollection ;)

        public CollectionParameter(string name, IEnumerable<T> initialValue, IModifiableParameterOwner owner, bool isSet = false)
        {
            Name = name;
            InitialValue = CreateCollection(initialValue, isSet);
            _value = InitialValue;
            owner.AddParameter(this);
        }

        public void AddValue(T item)
        {
            _value.Add(item);
        }

        public void Reset()
        {
            _value = InitialValue;
        }

        private static ICollection<T> CreateCollection(IEnumerable<T> initialValue, bool isSet)
        {
            return isSet ? (ICollection<T>) new HashSet<T>(initialValue) : new List<T>(initialValue);
        }
    }
}
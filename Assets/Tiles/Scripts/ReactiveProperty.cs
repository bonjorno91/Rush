using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    [Serializable]
    public class ReactiveProperty<T> where T : IEquatable<T>
    {
        public delegate void OnReactivePropertyChanged<in TValue>(TValue value);
        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    Notify(_value);
                }
            }
        }

        public ReactiveProperty(T value) => _value = value;

        [SerializeField] private T _value;

        private HashSet<OnReactivePropertyChanged<T>> _observers = new();

        public void Subscribe(OnReactivePropertyChanged<T> reactivePropertyObserver) => _observers.Add(reactivePropertyObserver);

        public void Unsubscribe(OnReactivePropertyChanged<T> reactivePropertyObserver) => _observers.Remove(reactivePropertyObserver);

        private void Notify(T value)
        {
            if (_observers?.Count > 0)
                foreach (var observer in _observers)
                    observer?.Invoke(value);
        }
    }
}
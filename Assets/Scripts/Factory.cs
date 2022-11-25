using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public abstract class Factory<TKey, TValue> : InitializeOnStartSO where TValue : Component
{
    private class ValueStore<TEntryKey, TEntryValue> where TEntryValue : Object
    {
        internal class Entry<TValueType> : IValueEntry<TValueType>
        {
            private readonly Action<Entry<TValueType>> _releaseAction;
            public TValueType Value { get; }

            public Entry(TValueType value, Action<Entry<TValueType>> releaseAction)
            {
                _releaseAction = releaseAction;
                Value = value;
            }

            public void Release()
            {
                _releaseAction?.Invoke(this);
            }
        }

        internal IReadOnlyCollection<TEntryValue> ActiveCollection => _activeValues;
        private readonly HashSet<TEntryValue> _activeValues = new();
        private readonly ObjectPool<Entry<TEntryValue>> _objectPool;
        private readonly TEntryValue _prefab;
        private readonly TEntryKey _key;
        private readonly Action<TEntryKey, TEntryValue> _onCreateEntry;
        private readonly Action<TEntryKey, TEntryValue> _onGetEntry;
        private readonly Action<TEntryKey, TEntryValue> _onReleaseEntry;
        private readonly Action<TEntryKey, TEntryValue> _onDestroyEntry;

        public ValueStore(TEntryValue prefab,
            TEntryKey key,
            Action<TEntryKey, TEntryValue> onCreateEntry,
            Action<TEntryKey, TEntryValue> onGetEntry,
            Action<TEntryKey, TEntryValue> onReleaseEntry,
            Action<TEntryKey, TEntryValue> onDestroyEntry)
        {
            _prefab = prefab;
            _key = key;
            _onCreateEntry = onCreateEntry;
            _onGetEntry = onGetEntry;
            _onReleaseEntry = onReleaseEntry;
            _onDestroyEntry = onDestroyEntry;

            _objectPool = new ObjectPool<Entry<TEntryValue>>(OnCreate, OnGet, OnRelease, OnDestroy);
        }

        public IValueEntry<TEntryValue> Get()
        {
            return _objectPool.Get();
        }

        private Entry<TEntryValue> OnCreate()
        {
            var instance = Instantiate(_prefab);
            var entry = new Entry<TEntryValue>(instance, _objectPool.Release);
            _onCreateEntry(_key, instance);
            _activeValues.Add(entry.Value);
            return entry;
        }

        private void OnGet(Entry<TEntryValue> obj)
        {
            _onGetEntry(_key, obj.Value);
            _activeValues.Add(obj.Value);
        }

        private void OnRelease(Entry<TEntryValue> obj)
        {
            _onReleaseEntry(_key, obj.Value);
            _activeValues.Remove(obj.Value);
        }

        private void OnDestroy(Entry<TEntryValue> obj)
        {
            _activeValues.Remove(obj.Value);
            _onDestroyEntry(_key, obj.Value);
        }
    }

    [field: SerializeField] protected FactoryConfig<TKey, TValue> Config { get; private set; }
    private readonly Dictionary<TKey, ValueStore<TKey, TValue>> _valueStores = new();
    private readonly List<TValue> _activeCollection = new();
    private bool _isDirty = true;

    public virtual bool TryGet(TKey key,Vector3 position,Quaternion rotation,out IValueEntry<TValue> result)
    {
        result = null;

        if (!Config.Entries.ContainsKey(key)) return false;

        if (!_valueStores.ContainsKey(key))
        {
            var valueStore = new ValueStore<TKey, TValue>(Config.Entries[key], key, OnCreate, OnGet, OnRelease, OnRemove);
            _valueStores.Add(key, valueStore);
        }

        result = _valueStores[key].Get();
        result.Value.transform.position = position;
        result.Value.transform.rotation = rotation;
        _isDirty = true;
        return true;
    }

    public bool TryGetActive(TKey key, out IReadOnlyCollection<TValue> values)
    {
        values = default;
        if (!_valueStores.ContainsKey(key)) return false;

        values = _valueStores[key].ActiveCollection;
        return true;
    }

    public bool TryGetAllActive(out IReadOnlyCollection<TValue> values)
    {
        values = default;
        if (_valueStores.Keys.Count == 0) return false;
        
        if (_isDirty)
        {
            _activeCollection.Clear();
            
            foreach (var storesValue in _valueStores.Values)
            {
                _activeCollection.AddRange(storesValue.ActiveCollection);
            }
            
            _isDirty = false;
        }
        
        values = _activeCollection;
        return true;
    }

    protected virtual void OnCreate(TKey key, TValue value)
    {
    }

    protected virtual void OnGet(TKey key, TValue value)
    {
    }

    protected virtual void OnRelease(TKey key, TValue value)
    {
    }

    protected virtual void OnRemove(TKey key, TValue value)
    {
    }
}
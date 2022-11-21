using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryConfig<TKey,TValue> : InitializeOnStartSO
{
    [Header("Note, keys must be unique!")]
    [SerializeField] private PrefabEntry<TKey, TValue>[] _prefabEntries;

    public IReadOnlyDictionary<TKey, TValue> Entries => _entries;
    private readonly Dictionary<TKey, TValue> _entries = new();

    public override void Initialize()
    {
        if (_prefabEntries != null)
        {
            foreach (var prefabEntry in _prefabEntries)
            {
                _entries[prefabEntry.Key] = prefabEntry.Value;
            }
        }
    }
    
    [Serializable]
    private class PrefabEntry<TEntryKey,TEntryValue>
    {
        [field: SerializeField] public TEntryKey Key { get; private set; }
        [field: SerializeField] public TEntryValue Value { get; private set; }
    }
}
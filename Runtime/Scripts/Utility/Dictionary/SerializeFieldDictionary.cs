using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeFieldDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = value; // Update existing value
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public void Remove(TKey key)
    {
        dictionary.Remove(key);
    }

    // Indexer to access dictionary items
    public TValue this[TKey key]
    {
        get => dictionary.GetValueOrDefault(key);
        set => Add(key, value);
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return new Dictionary<TKey, TValue>(dictionary);
    }

    public List<TKey> Keys => new List<TKey>(dictionary.Keys);
    public List<TValue> Values => new List<TValue>(dictionary.Values);
}

using System;
using System.Collections.Generic;

[Serializable]
public struct SerializedDictionnary<TKey, TValue>
{
    public List<TKey> Keys;
    public List<TValue> Values;

    public void Add(TKey key, TValue value)
    {
        Keys.Add(key);
        Values.Add(value);
    }
    
    /// <summary>
    /// Let you use the [] operator to acces element in the dictionnary, return the value that is stored at that key.
    /// </summary>
    /// <param name="key"> The key used to access the value </param>
    /// <exception cref="KeyNotFoundException"> If the key isn't in the dictionnary, the KeyNotFound get thrown</exception>
    public TValue this[TKey key]
    {
        get
        {
            var index = Keys.FindIndex(k => k.Equals(key));
            if (index != -1)
            {
                return Values[index];
            }
            throw new KeyNotFoundException();
        }
        set
        {
            var index = Keys.FindIndex(k => k.Equals(key));
            if (index != -1)
            {
                Values[index] = value;
                return;
            }
            throw new KeyNotFoundException();
        }
    }
    
    /// <summary>
    /// Let you use the [] operator to acces element in the dictionnary, return the key that is stored at that value.
    /// </summary>
    /// <param name="key"> The value used to access the key </param>
    /// <exception cref="KeyNotFoundException"> If the value isn't in the dictionnary, the KeyNotFound get thrown</exception>
    public TKey this[TValue key]
    {
        get
        {
            var index = Values.FindIndex(k => k.Equals(key));
            if (index != -1)
            {
                return Keys[index];
            }
            throw new KeyNotFoundException();
        }
        set
        {
            var index = Values.FindIndex(k => k.Equals(key));
            if (index != -1)
            {
                Keys[index] = value;
                return;
            }
            throw new KeyNotFoundException();
        }
    }
    
}